using BoxDropperServer.Utils;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Strategies;
using DropBoxExercise.Server.Utils.ClientMessageUtils;

namespace BoxDropperServer.Strategies.ClientMessageStrategies;

public class ChangedDirectoryClientMessageStrategy : IClientMessageStrategy
{
    private readonly ClientMessageDto _messageDto;

    public ChangedDirectoryClientMessageStrategy(ClientMessageDto messageDto)
    {
        _messageDto = messageDto ?? throw new ArgumentNullException(nameof(messageDto));
    }

    public void Execute(string directoryPath)
    {
        string fullPath = Path.Combine(directoryPath, _messageDto.ItemName);

        try
        {
            DirectoryManager.CreateDirectory(fullPath); // Placeholder for directory change handling
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling directory change '{fullPath}': {ex.Message}");
            throw; // Rethrow the exception to propagate it
        }
    }

    public void Validate()
    {
        if (_messageDto.ItemData != null)
        {
            throw new ArgumentException("File data should be null for directory change.");
        }

        if (!ClientMessageValidator.ValidateDirectoryName(_messageDto.ItemName))
        {
            throw new ArgumentException("Invalid directory name.");
        }
    }
}
