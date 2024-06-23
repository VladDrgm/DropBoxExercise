using DropBoxExercise.Client.Domain;
using FluentAssertions;
using Moq;

namespace DropBoxExercise.Client.UnitTests;

public class ClientTests
{
    private readonly string _testDirectory;
    private readonly Domain.Client _client;
    private readonly Mock<IFileSystemObserver> _observerMock;

    public ClientTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);

        _client = new Domain.Client(_testDirectory);
        _observerMock = new Mock<IFileSystemObserver>();
    }

    [Fact]
    public void StartMonitoring_CreatesWatcher()
    {
        _client.AddObserver(_observerMock.Object);
        _client.StartMonitoring();

        GetWatcher(_client).Should().NotBeNull();
        GetWatcher(_client).EnableRaisingEvents.Should().BeTrue();
    }

    [Fact]
    public void NotifyCreated_NotifiesObserver()
    {
        _client.AddObserver(_observerMock.Object);
        _client.StartMonitoring();

        string testFile = Path.Combine(_testDirectory, "test.txt");
        File.WriteAllText(testFile, "test content");

        System.Threading.Thread.Sleep(100); // Wait for file system events to propagate

        _observerMock.Verify(o => o.OnFileCreated(testFile), Times.Once);
    }

    [Fact]
    public void NotifyChanged_NotifiesObserver()
    {
        _client.AddObserver(_observerMock.Object);
        _client.StartMonitoring();

        string testFile = Path.Combine(_testDirectory, "test.txt");
        File.WriteAllText(testFile, "initial content");

        System.Threading.Thread.Sleep(100); // Wait for file system events to propagate

        File.WriteAllText(testFile, "modified content");

        System.Threading.Thread.Sleep(100); // Wait for file system events to propagate
        
        _observerMock.Verify(o => o.OnFileChanged(testFile));
    }

    [Fact]
    public void NotifyRenamed_NotifiesObserver()
    {
        _client.AddObserver(_observerMock.Object);
        _client.StartMonitoring();

        string oldFilePath = Path.Combine(_testDirectory, "old.txt");
        string newFilePath = Path.Combine(_testDirectory, "new.txt");
        File.WriteAllText(oldFilePath, "test content");

        System.Threading.Thread.Sleep(100); // Wait for file system events to propagate

        File.Move(oldFilePath, newFilePath);

        System.Threading.Thread.Sleep(100); // Wait for file system events to propagate

        _observerMock.Verify(o => o.OnFileRenamed(oldFilePath, newFilePath), Times.Once);
    }

    [Fact]
    public void NotifyDeleted_NotifiesObserver()
    {
        _client.AddObserver(_observerMock.Object);
        _client.StartMonitoring();

        string testFile = Path.Combine(_testDirectory, "test.txt");
        File.WriteAllText(testFile, "test content");

        System.Threading.Thread.Sleep(100); // Wait for file system events to propagate

        File.Delete(testFile);

        System.Threading.Thread.Sleep(100); // Wait for file system events to propagate

        _observerMock.Verify(o => o.OnDeleted(testFile), Times.Once);
    }

    [Fact]
    public void StartMonitoring_WithoutObserver_DoesNotThrow()
    {
        _client.Invoking(c => c.StartMonitoring()).Should().NotThrow();
    }

    [Fact]
    public void StopMonitoring_WithoutWatcher_DoesNotThrow()
    {
        _client.Invoking(c => c.StopMonitoring()).Should().NotThrow();
    }

    [Fact]
    public void AddObserver_SetsObserver()
    {
        _client.AddObserver(_observerMock.Object);

        GetObserver(_client).Should().Be(_observerMock.Object);
    }

    [Fact]
    public void RemoveObserver_ClearsObserver()
    {
        _client.AddObserver(_observerMock.Object);
        _client.RemoveObserver(_observerMock.Object);

        GetObserver(_client).Should().BeNull();
    }

    [Fact]
    public void IsTemporaryFile_ReturnsTrueForTemporaryFile()
    {
        string tempFilePath = "test~";
        bool result = InvokeIsTemporaryFile(tempFilePath);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsTemporaryFile_ReturnsFalseForNonTemporaryFile()
    {
        string filePath = "test.txt";
        bool result = InvokeIsTemporaryFile(filePath);

        result.Should().BeFalse();
    }

    private static FileSystemWatcher GetWatcher(Domain.Client client)
    {
        var watcherField = typeof(Domain.Client).GetField("_watcher", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (FileSystemWatcher)watcherField.GetValue(client);
    }

    private static IFileSystemObserver GetObserver(Domain.Client client)
    {
        var observerField = typeof(Domain.Client).GetField("_observer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (IFileSystemObserver)observerField.GetValue(client);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
    
    private bool InvokeIsTemporaryFile(string filePath)
    {
        var methodInfo = typeof(Domain.Client).GetMethod("IsTemporaryFile", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (bool)methodInfo.Invoke(_client, new object[] { filePath });
    }
}
