using System.Text;
using System.Text.Json;
using DropBoxExercise.Client.Dto;
using DropBoxExercise.Client.Utils;
using DropBoxExercise.CommonUtils.Enums;
using FluentAssertions;

namespace DropBoxExercise.Client.UnitTests;

public class ServerMessageParserTests
{
    [Fact]
    public void When_ParsingMessage_Expect_CorrectBytes()
    {
        // Arrange
        var message = new ServerMessageDto(ChangeType.FileChanged, "test.txt", [1, 2, 3, 4], "oldTest.txt", "path", "path");
        
        // Act
        var result = ServerMessageParser.ParseMessage(message);
        
        // Assert
        var actualJson = Encoding.UTF8.GetString(result);
        var actualMessage = JsonSerializer.Deserialize<ServerMessageDto>(actualJson);
        result.Length.Should().BeGreaterThan(0);
        result.Should().NotBeNull();
        actualMessage.Should().BeEquivalentTo(message);
    }
}