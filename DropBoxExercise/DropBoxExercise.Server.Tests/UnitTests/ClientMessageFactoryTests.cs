using DropBoxExercise.CommonUtils.Enums;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Strategies;
using DropBoxExercise.Server.Strategies.ClientMessageStrategies.Changed;
using DropBoxExercise.Server.Strategies.ClientMessageStrategies.Created;
using DropBoxExercise.Server.Strategies.ClientMessageStrategies.Deleted;
using DropBoxExercise.Server.Strategies.ClientMessageStrategies.Renamed;

namespace DropBoxExercise.Server.Tests.UnitTests;

public class ClientMessageFactoryTests
{
    [Fact]
    public void When_ReceivingClientMessageDto_WithTypeCreated_Then_ReturnCreatedClientMessageStrategy()
    {
        // Arrange
        var clientMessage = new ClientMessageDto(
            ChangeType.FileCreated,
            "test.txt",
            new byte[] { 0x00, 0x01, 0x02 },
            null,
            "test.txt",
            null
        );
        
        // Act
        var result = ClientMessageStrategyFactory.Build(clientMessage);
        
        // Assert
        Assert.IsType<CreatedFileClientMessageStrategy>(result);
    }
    
    [Fact]
    public void When_ReceivingClientMessageDto_WithTypeChanged_Then_ReturnChangedClientMessageStrategy()
    {
        // Arrange
        var clientMessage = new ClientMessageDto(
            ChangeType.FileChanged,
            "test.txt",
            new byte[] { 0x00, 0x01, 0x02 },
            null,
            "test.txt",
            null
        );
        
        // Act
        var result = ClientMessageStrategyFactory.Build(clientMessage);
        
        // Assert
        Assert.IsType<ChangedFileClientMessageStrategy>(result);
    }
    
    [Fact]
    public void When_ReceivingClientMessageDto_WithTypeRenamed_Then_ReturnRenamedClientMessageStrategy()
    {
        // Arrange
        var clientMessage = new ClientMessageDto(
            ChangeType.FileRenamed,
            "test.txt",
            new byte[] { 0x00, 0x01, 0x02 },
            null,
            "test.txt",
            null
        );
        
        // Act
        var result = ClientMessageStrategyFactory.Build(clientMessage);
        
        // Assert
        Assert.IsType<RenamedFileClientMessageStrategy>(result);
    }
    
    [Fact]
    public void When_ReceivingClientMessageDto_WithTypeDeleted_Then_ReturnDeletedClientMessageStrategy()
    {
        // Arrange
        var clientMessage = new ClientMessageDto(
            ChangeType.Deleted,
            "test.txt",
            new byte[] { 0x00, 0x01, 0x02 },
            null,
            "test.txt",
            null
        );
        
        // Act
        var result = ClientMessageStrategyFactory.Build(clientMessage);
        
        // Assert
        Assert.IsType<DeletedClientMessageStrategy>(result);
    }
    
}