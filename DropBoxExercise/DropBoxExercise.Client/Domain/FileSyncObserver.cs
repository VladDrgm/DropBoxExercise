using System.Net.Sockets;
using DropBoxExercise.Client.Dto;
using DropBoxExercise.Client.Utils;
using DropBoxExercise.CommonUtils.Enums;

namespace DropBoxExercise.Client.Domain
{
    public class FileSyncObserver : IFileSystemObserver
    {
        private readonly string _serverAddress;
        private readonly int _serverPort;

        public FileSyncObserver(string serverAddress, int serverPort)
        {
            _serverAddress = serverAddress;
            _serverPort = serverPort;
        }

        public async void OnFileCreated(string filePath)
        {
            SendMessage(new ServerMessageDto
                (
                    ChangeType : ChangeType.FileCreated,
                    ItemName: Path.GetFileName(filePath),
                    ItemData: await File.ReadAllBytesAsync(filePath),
                    OldItemName: null,
                    ItemPath: filePath,
                    OldItemPath: null
                ));
        }

        public async void OnFileChanged(string filePath)
        {
            SendMessage(new ServerMessageDto
                (
                    ChangeType : ChangeType.FileChanged,
                    ItemName: Path.GetFileName(filePath),
                    ItemData: await File.ReadAllBytesAsync(filePath),
                    OldItemName: null,
                    ItemPath: filePath,
                    OldItemPath: null
                ));
        }

        public async void OnFileRenamed(string oldItemPath, string newItemPath)
        {
            SendMessage(new ServerMessageDto
                (
                    ChangeType: ChangeType.FileRenamed,
                    ItemName: Path.GetFileName(newItemPath),
                    ItemData: await File.ReadAllBytesAsync(newItemPath),
                    OldItemName: Path.GetFileName(oldItemPath),
                    ItemPath: newItemPath,
                    OldItemPath : oldItemPath
                ));
        }

        public void OnFolderCreated(string folderPath)
        {
            SendMessage(new ServerMessageDto
                (
                    ChangeType: ChangeType.FolderCreated,
                    ItemName: new DirectoryInfo(folderPath).Name,
                    ItemData: null,
                    OldItemName: null,
                    ItemPath: folderPath,
                    OldItemPath: null
                ));
        }

        public void OnFolderChanged(string folderPath)
        {
            SendMessage(new ServerMessageDto
                (
                    ChangeType: ChangeType.FolderChanged,
                    ItemName: new DirectoryInfo(folderPath).Name,
                    ItemData: null,
                    OldItemName: null,
                    ItemPath: folderPath,
                    OldItemPath: null
                ));
        }

        public void OnFolderRenamed(string oldFolderPath, string newFolderPath)
        {
            SendMessage(new ServerMessageDto
                (
                    ChangeType: ChangeType.FolderRenamed,
                    ItemName: new DirectoryInfo(newFolderPath).Name,
                    ItemData: null,
                    OldItemName: new DirectoryInfo(oldFolderPath).Name,
                    ItemPath: newFolderPath,
                    OldItemPath: oldFolderPath
                ));
        }

        public void OnDeleted(string folderPath)
        {
            SendMessage(new ServerMessageDto
                (
                    ChangeType: ChangeType.Deleted,
                    ItemName: null,
                    ItemData: null,
                    OldItemName: null,
                    ItemPath: folderPath,
                    OldItemPath: null
                ));
        }

        private async void SendMessage(ServerMessageDto message)
        {
            try
            {
                var messageBytes = ServerMessageParser.ParseMessage(message);

                using var tcpClient = new TcpClient(_serverAddress, _serverPort);
                using var networkStream = tcpClient.GetStream();

                var messageLengthBytes = BitConverter.GetBytes(messageBytes.Length);
                await networkStream.WriteAsync(messageLengthBytes, 0, messageLengthBytes.Length);
                await networkStream.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error sending message: {0}", ex.Message);
            }
        }
    }
}

