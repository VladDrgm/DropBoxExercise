using BoxDropperServer.Utils;
using DropBoxExercise.Server.Dto;

namespace DropBoxExercise.Server.Strategies.ClientMessageStrategies.Deleted;

public class DeletedClientMessageStrategy : IClientMessageStrategy
{
    private readonly ClientMessageDto _messageDto;

    public DeletedClientMessageStrategy(ClientMessageDto messageDto)
    {
        _messageDto = messageDto;
    }

    public void Execute(string destinationDirectoryPath)
    {
        
        var fullPath = destinationDirectoryPath;
        
        var originPathDirectories = _messageDto.ItemPath.Split("/");
        var targetPathDirectories = destinationDirectoryPath.Split("/");
        
        var itemIsInSubdirectory = originPathDirectories.Length > targetPathDirectories.Length;
        
        if (itemIsInSubdirectory)
        {
            var subdirectoryDepth = originPathDirectories.Length - targetPathDirectories.Length;
            
            for ( int i = 0; i < subdirectoryDepth; i++)
            {
                fullPath = Path.Combine(fullPath, originPathDirectories[targetPathDirectories.Length + i]);
            }
        }
        
        if (File.Exists(fullPath))
        {
            DirectoryManager.DeleteFile(fullPath);
        }
        
        if (Directory.Exists(fullPath))
        {
            DirectoryManager.DeleteDirectory(fullPath);
        }
    }
    
    public void Validate()
    {
        var validate = _messageDto.ItemData is null;
        
        if (!validate)
        {
            throw new Exception("Delete action failed.");
        }
    }
}
