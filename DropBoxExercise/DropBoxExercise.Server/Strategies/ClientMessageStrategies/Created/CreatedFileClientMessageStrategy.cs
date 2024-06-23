using BoxDropperServer.Utils;
using DropBoxExercise.CommonUtils.PathBuilder;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Utils.ClientMessageUtils;

namespace DropBoxExercise.Server.Strategies.ClientMessageStrategies.Created;

public class CreatedFileClientMessageStrategy : IClientMessageStrategy
{
    
    private readonly ClientMessageDto _messageDto;

    public CreatedFileClientMessageStrategy(ClientMessageDto messageDto)
    {
        _messageDto = messageDto;
    }

    public void Execute(string destinationDirectoryPath)
    {
        var path = PathBuilder.BuildFilePath(_messageDto.ItemPath, destinationDirectoryPath);
        
        DirectoryManager.SaveFile(path, _messageDto.ItemData!);
    }
    
    public void Validate()
    {
        var fileDataIsValid = _messageDto.ItemData is not null;
        var fileNameIsValid = ClientMessageValidator.ValidateFileName(_messageDto.ItemName);

        var isValid = fileDataIsValid && fileNameIsValid;
        
        if (!isValid)
        {
            throw new Exception("Create action failed.");
        }
    }
}
