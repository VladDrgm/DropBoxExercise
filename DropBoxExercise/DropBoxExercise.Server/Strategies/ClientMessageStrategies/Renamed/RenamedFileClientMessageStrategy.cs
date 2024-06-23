using BoxDropperServer.Utils;
using DropBoxExercise.CommonUtils.PathBuilder;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Utils.ClientMessageUtils;

namespace DropBoxExercise.Server.Strategies.ClientMessageStrategies.Renamed;

public class RenamedFileClientMessageStrategy : IClientMessageStrategy
{
    
    private readonly ClientMessageDto _messageDto;

    public RenamedFileClientMessageStrategy(ClientMessageDto messageDto)
    {
        _messageDto = messageDto;
    }

    public void Execute(string directoryPath)
    {
        var oldPath = PathBuilder.BuildFilePath(_messageDto.OldItemPath, directoryPath);
        DirectoryManager.DeleteFile(oldPath);
        
        var path = PathBuilder.BuildFilePath(_messageDto.ItemPath, directoryPath);
        DirectoryManager.SaveFile(path, _messageDto.ItemData!);
    }

    public void Validate()
    {
        var fileDataIsValid = _messageDto.ItemData is not null;
        
        var oldFileNameIsValid = _messageDto.OldItemName is not null && 
                                 ClientMessageValidator.ValidateFileName(_messageDto.OldItemName);

        var fileNameIsValid = ClientMessageValidator.ValidateFileName(_messageDto.ItemName);

        var validate = fileDataIsValid && oldFileNameIsValid && fileNameIsValid;

        if (!validate)
        {
            throw new Exception("Rename action failed.");
        }
    }
}
