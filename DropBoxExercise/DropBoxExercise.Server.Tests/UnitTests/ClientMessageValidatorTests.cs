using DropBoxExercise.CommonUtils.Enums;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Utils.ClientMessageUtils;
using FluentAssertions;

namespace DropBoxExercise.Server.Tests.UnitTests;

public class ClientMessageValidatorTests
{
    
    // make this a theory
    [Theory]
    [InlineData(ChangeType.FileCreated)]
    [InlineData(ChangeType.Deleted)]
    [InlineData(ChangeType.FileChanged)]
    [InlineData(ChangeType.FileRenamed)]
    public void When_ClientMessage_HasCorrectChangeType_ValidationPasses(ChangeType changeType)
    {
        // Arrange
        var clientMessage = new ClientMessageDto(
            changeType,
            "test.txt",
            new byte[] { 0x00, 0x01, 0x02 },
            null,
            "test.txt",
            null
            );
        
        // Act
        var exception = Record.Exception(() => ClientMessageValidator.Validate(clientMessage));
        
        // Assert
        exception.Should().BeNull();
    }
}