using DropBoxExercise.Client.Interfaces;

namespace DropBoxExercise.Client.Domain;


public class Client
{
    private readonly string _sourceDirectory;
    private IFileSystemObserver? _observer;
    private FileSystemWatcher _watcher;

    public Client(string sourceDirectory, FileSystemWatcher watcher, IFileSystemObserver? observer)
    {
        _sourceDirectory = sourceDirectory;
        _watcher = watcher;
        _observer = observer;
    }

    public void RemoveObserver(IFileSystemObserver observer)
    {
        _observer = null;
    }

    public void StartMonitoring()
    {
        _watcher.Created += OnCreated;
        _watcher.Changed += OnChanged;
        _watcher.Renamed += OnRenamed;
        _watcher.Deleted += OnDeleted;
        Console.WriteLine("Monitoring directory: {0}", _sourceDirectory);
    }

    public void StopMonitoring()
    {
        _watcher.EnableRaisingEvents = false;
        _watcher.Dispose();
    }

    private void NotifyCreated(string path)
    {
        if (Directory.Exists(path))
        {
            _observer?.OnFolderCreated(path);
        }
        else
        {
            _observer?.OnFileCreated(path);
        }
    }

    private void NotifyChanged(string path)
    {
        if (Directory.Exists(path))
        {
            _observer?.OnFolderChanged(path);
        }
        else
        {
            _observer?.OnFileChanged(path);
        }
    }

    private void NotifyRenamed(string oldPath, string newPath)
    {
        if (Directory.Exists(newPath))
        {
            _observer?.OnFolderRenamed(oldPath, newPath);
        }
        else
        {
            _observer?.OnFileRenamed(oldPath, newPath);
        }
    }

    private void NotifyDeleted(string path, bool directoryFlag)
    {
        if (directoryFlag)
        {
            _observer?.OnDeleted(path);
        }
        else
        {
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
