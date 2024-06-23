namespace DropBoxExercise.Client.Domain;

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


public class Client
{
    private readonly string _sourceDirectory;
    private IFileSystemObserver? _observer;
    private FileSystemWatcher _watcher;

    public Client(string sourceDirectory)
    {
        _sourceDirectory = sourceDirectory;
    }

    public void AddObserver(IFileSystemObserver observer)
    {
        _observer = observer;
    }

    public void AddWatcher(FileSystemWatcher watcher)
    {
        _watcher = watcher;
    }

    public void RemoveObserver(IFileSystemObserver observer)
    {
        _observer = null;
    }

    public void StartMonitoring()
    {
        var watcher = new FileSystemWatcher(_sourceDirectory)
        {
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };
        watcher.Created += OnCreated;
        watcher.Changed += OnChanged;
        watcher.Renamed += OnRenamed;
        watcher.Deleted += OnDeleted;

        AddWatcher(watcher);
        Console.WriteLine("Monitoring directory: {0}", _sourceDirectory);
    }

    public void StopMonitoring()
    {
        if (_watcher != null)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
        }
    }

    private void NotifyCreated(string path)
    {
        if (Directory.Exists(path))
        {
            Console.WriteLine($"Folder created: {path}");
            _observer?.OnFolderCreated(path);
        }
        else
        {
            Console.WriteLine($"File created: {path}");
            _observer?.OnFileCreated(path);
        }
    }

    private void NotifyChanged(string path)
    {
        if (Directory.Exists(path))
        {
            Console.WriteLine($"Folder changed: {path}");
            _observer?.OnFolderChanged(path);
        }
        else
        {
            Console.WriteLine($"File changed: {path}");
            _observer?.OnFileChanged(path);
        }
    }

    private void NotifyRenamed(string oldPath, string newPath)
    {
        if (Directory.Exists(newPath))
        {
            Console.WriteLine($"Folder renamed from {oldPath} to {newPath}");
            _observer?.OnFolderRenamed(oldPath, newPath);
        }
        else
        {
            Console.WriteLine($"File renamed from {oldPath} to {newPath}");
            _observer?.OnFileRenamed(oldPath, newPath);
        }
    }

    private void NotifyDeleted(string path, bool directoryFlag)
    {
        if (directoryFlag)
        {
            Console.WriteLine($"Folder deleted: {path}");
            _observer?.OnDeleted(path);
        }
        else
        {
            Console.WriteLine($"File deleted: {path}");
            _observer?.OnDeleted(path);
        }
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        if (IsTemporaryFile(e.FullPath) || (!File.Exists(e.FullPath) && !Directory.Exists(e.FullPath)))
        {
            return;
        }

        NotifyCreated(e.FullPath);
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (IsTemporaryFile(e.FullPath) || (!File.Exists(e.FullPath) && !Directory.Exists(e.FullPath)))
        {
            return;
        }

        NotifyChanged(e.FullPath);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        NotifyRenamed(e.OldFullPath, e.FullPath);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        if (IsTemporaryFile(e.FullPath))
        {
            return;
        }

        try
        {
            FileAttributes attributes = File.GetAttributes(e.FullPath);

            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                NotifyDeleted(e.FullPath, true);
            }
            else
            {
                NotifyDeleted(e.FullPath, false);
            }
        }
        catch (Exception ex)
        {
            NotifyDeleted(e.FullPath, false);
        }
    }

    private bool IsTemporaryFile(string filePath)
    {
        return filePath.EndsWith("~");
    }
}
