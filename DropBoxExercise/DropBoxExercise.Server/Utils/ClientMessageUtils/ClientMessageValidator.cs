using DropBoxExercise.CommonUtils.Enums;
using DropBoxExercise.Server.Dto;

namespace DropBoxExercise.Server.Utils.ClientMessageUtils;

public static class ClientMessageValidator
{
    public static void Validate(ClientMessageDto messageDto)
    {
        var isChangeTypeValid = messageDto.ChangeType == ChangeType.FileCreated ||
                                messageDto.ChangeType == ChangeType.FileChanged ||
                                messageDto.ChangeType == ChangeType.FileRenamed ||
                                messageDto.ChangeType == ChangeType.Deleted ||
                                messageDto.ChangeType == ChangeType.FolderCreated ||
                                messageDto.ChangeType == ChangeType.FolderRenamed ||
                                messageDto.ChangeType == ChangeType.FolderChanged;
        
        if (!isChangeTypeValid)
        {
            throw new Exception("Invalid change type");
        }
    }
    
    public static bool ValidateFileName(string fileName)
    {
        return fileName.Length > 0;
    }
    
    public static bool ValidateDirectoryName(string directoryPath)
    {
        return directoryPath.Length > 0;
    }
}
