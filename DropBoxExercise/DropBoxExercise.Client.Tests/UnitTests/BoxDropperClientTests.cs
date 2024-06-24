using DropBoxExercise.Client.Interfaces;
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
        
        _observerMock = new Mock<IFileSystemObserver>();

        _client = CreateMockedClient();
    }
    
    [Fact]
    public void StartMonitoring_CreatesWatcher()
    {
        _client.StartMonitoring();

        GetWatcher(_client).Should().NotBeNull();
        GetWatcher(_client).EnableRaisingEvents.Should().BeTrue();
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
        GetObserver(_client).Should().Be(_observerMock.Object);
    }

    [Fact]
    public void RemoveObserver_ClearsObserver()
    {
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
    
    private bool InvokeIsTemporaryFile(string filePath)
    {
        var methodInfo = typeof(Domain.Client).GetMethod("IsTemporaryFile", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (bool)methodInfo.Invoke(_client, new object[] { filePath });
    }
    
    private Domain.Client CreateMockedClient()
    {
        var watcher = new FileSystemWatcher(_testDirectory)
        {
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };
        return new Domain.Client(_testDirectory, watcher, _observerMock.Object);
    }
}
