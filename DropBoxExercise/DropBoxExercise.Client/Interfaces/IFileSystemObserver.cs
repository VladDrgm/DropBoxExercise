namespace DropBoxExercise.Client.Interfaces;

public interface IFileSystemObserver
{
    void OnFileCreated(string filePath);
    void OnFileChanged(string filePath);
    void OnFileRenamed(string oldFilePath, string newFilePath);
    void OnDeleted(string filePath);

    void OnFolderCreated(string folderPath);
    void OnFolderChanged(string folderPath);
    void OnFolderRenamed(string oldFolderPath, string newFolderPath);
}