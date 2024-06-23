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
        Console.WriteLine("Checking if file exists: {0}", _messageDto.ItemPath);
        
        var fullPath = destinationDirectoryPath;
        
        var originPathDirectories = _messageDto.ItemPath.Split("/");
        
        // these are the directories in the target path
        var targetPathDirectories = destinationDirectoryPath.Split("/");
        
        var fileIsInSubdirectory = originPathDirectories.Length > targetPathDirectories.Length;
        
        if (fileIsInSubdirectory)
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
