using BoxDropperServer.Strategies.ClientMessageStrategies;
using DropBoxExercise.CommonUtils.Enums;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Strategies.ClientMessageStrategies.Changed;
using DropBoxExercise.Server.Strategies.ClientMessageStrategies.Created;
using DropBoxExercise.Server.Strategies.ClientMessageStrategies.Deleted;
using DropBoxExercise.Server.Strategies.ClientMessageStrategies.Renamed;

namespace DropBoxExercise.Server.Strategies;

public static class ClientMessageStrategyFactory
{
    public static IClientMessageStrategy Build(ClientMessageDto messageDto)
    {
        return messageDto.ChangeType switch
        {
            ChangeType.FileCreated => new CreatedFileClientMessageStrategy(messageDto),
            ChangeType.FileChanged => new ChangedFileClientMessageStrategy(messageDto),
            ChangeType.FileRenamed => new RenamedFileClientMessageStrategy(messageDto),
            ChangeType.Deleted => new DeletedClientMessageStrategy(messageDto),
            ChangeType.FolderCreated => new CreatedDirectoryClientMessageStrategy(messageDto),
            ChangeType.FolderRenamed => new RenamedDirectoryClientMessageStrategy(messageDto),
            ChangeType.FolderChanged => new ChangedDirectoryClientMessageStrategy(messageDto),
            _ => throw new Exception("Invalid change type")
        };
    }
}
