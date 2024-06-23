using BoxDropperServer.Utils;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Utils.ClientMessageUtils;

namespace DropBoxExercise.Server.Strategies.ClientMessageStrategies.Created;

public class CreatedDirectoryClientMessageStrategy : IClientMessageStrategy
{
    private readonly ClientMessageDto _messageDto;

    public CreatedDirectoryClientMessageStrategy(ClientMessageDto messageDto)
    {
        _messageDto = messageDto ?? throw new ArgumentNullException(nameof(messageDto));
    }

    public void Execute(string destinationDirectoryPath)
    {
        var originPathDirectories = _messageDto.ItemPath.Split("/");
        var targetPathDirectories = destinationDirectoryPath.Split("/");
        
        var directoryIsInSubdirectory = originPathDirectories.Length > targetPathDirectories.Length + 1;
        
        if (directoryIsInSubdirectory)
        {
            var subdirectoryDepth = originPathDirectories.Length - targetPathDirectories.Length + 1;
            
            for (int i = 0; i < subdirectoryDepth - 1; i++)
            {
                destinationDirectoryPath = Path.Combine(destinationDirectoryPath, originPathDirectories[targetPathDirectories.Length + i]);
            }
        }
        else
        {
            destinationDirectoryPath = Path.Combine(destinationDirectoryPath, _messageDto.ItemName);
        }
        
        Console.WriteLine("Creating directory: " + destinationDirectoryPath);

        DirectoryManager.CreateDirectory(destinationDirectoryPath);
    }

    public void Validate()
    {
        if (_messageDto.ItemData != null)
        {
            throw new ArgumentException("File data should be null for directory creation.");
        }

        if (!ClientMessageValidator.ValidateDirectoryName(_messageDto.ItemName))
        {
            throw new ArgumentException("Invalid directory name.");
        }
    }
}
    