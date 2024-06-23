using BoxDropperServer.Utils;
using DropBoxExercise.CommonUtils.PathBuilder;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Utils.ClientMessageUtils;

namespace DropBoxExercise.Server.Strategies.ClientMessageStrategies.Changed;

public class ChangedFileClientMessageStrategy : IClientMessageStrategy
{
    
    private readonly ClientMessageDto _messageDto;

    public ChangedFileClientMessageStrategy(ClientMessageDto messageDto)
    {
        _messageDto = messageDto;
    }

    public void Execute(string destinationDirectory)
    {
        var path = PathBuilder.BuildFilePath(_messageDto.ItemPath, destinationDirectory);
        
        DirectoryManager.SaveFile(path, _messageDto.ItemData!);
    }

    public void Validate()
    {
        var fileDataIsValid = _messageDto.ItemData is not null;

        var fileNameIsValid = ClientMessageValidator.ValidateFileName(_messageDto.ItemName);

        var validate = fileDataIsValid && fileNameIsValid;

        if (!validate)
        {
            throw new Exception("Change action failed.");
        }
    }
}
