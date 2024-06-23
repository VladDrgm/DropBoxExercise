using BoxDropperServer.Utils;
using DropBoxExercise.CommonUtils.PathBuilder;
using DropBoxExercise.Server.Dto;
using DropBoxExercise.Server.Utils.ClientMessageUtils;

namespace DropBoxExercise.Server.Strategies.ClientMessageStrategies.Renamed
{
    public class RenamedDirectoryClientMessageStrategy : IClientMessageStrategy
    {
        private readonly ClientMessageDto _messageDto;

        public RenamedDirectoryClientMessageStrategy(ClientMessageDto messageDto)
        {
            _messageDto = messageDto ?? throw new ArgumentNullException(nameof(messageDto));
        }
        
        public void Execute(string directoryPath)
        {
            string oldFullPath = PathBuilder.BuildFilePath(_messageDto.OldItemPath, directoryPath);
            string newFullPath = PathBuilder.BuildFilePath(_messageDto.ItemPath, directoryPath);

            try
            {
                DirectoryManager.CreateDirectory(newFullPath);

                // Move any files from the old directory into the new one
                string[] files = Directory.GetFiles(oldFullPath);
                foreach (string file in files)
                {
                    string destFile = Path.Combine(newFullPath, Path.GetFileName(file));
                    File.Move(file, destFile);
                }

                // Move any subdirectory from the old directory into the new one
                string[] subdirectories = Directory.GetDirectories(oldFullPath);
                foreach (string subdir in subdirectories)
                {
                    string destSubdir = Path.Combine(newFullPath, Path.GetFileName(subdir));
                    Directory.Move(subdir, destSubdir);
                }

                // Now delete the old directory
                Directory.Delete(oldFullPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error renaming folder '{oldFullPath}' to '{newFullPath}': {ex.Message}");
                throw; // Rethrow the exception to propagate it
            }
        }


        public void Validate()
        {
            if (_messageDto.ItemData != null)
            {
                throw new ArgumentException("File data should be null for folder rename.");
            }

            if (_messageDto.OldItemName == null || _messageDto.ItemName == null)
            {
                throw new ArgumentException("Old and current folder names must be provided.");
            }

            if (!ClientMessageValidator.ValidateDirectoryName(_messageDto.OldItemName) ||
                !ClientMessageValidator.ValidateDirectoryName(_messageDto.ItemName))
            {
                throw new ArgumentException("Invalid folder name.");
            }
        }
    }
}