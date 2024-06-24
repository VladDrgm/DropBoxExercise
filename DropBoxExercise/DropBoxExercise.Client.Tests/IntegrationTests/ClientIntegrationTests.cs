using DropBoxExercise.Client.Interfaces;
using FluentAssertions;
using Moq;

namespace DropBoxExercise.Client.IntegrationTests;

public class ClientIntegrationTests
{
    private readonly string _testDirectory;
    private readonly Domain.Client _client;
    private readonly Mock<IFileSystemObserver> _observerMock;

    public ClientIntegrationTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        
        _observerMock = new Mock<IFileSystemObserver>();

        _client = CreateMockedClient();
    }

    [Fact]
    public void NotifyCreated_NotifiesObserver()
    {
        // Arrange
        _client.StartMonitoring();

        var testFile = Path.Combine(_testDirectory, "test.txt");
        File.WriteAllText(testFile, "test content");
        
        // Act
        Thread.Sleep(100); // Wait for file system events to propagate
        
        // Assert
        _observerMock.Invocations.Should().Contain(i => i.Method.Name == "OnFileCreated" && i.Arguments[0].ToString() == testFile);
    }

    [Fact]
    public void NotifyChanged_NotifiesObserver()
    {
        // Arrange
        _client.StartMonitoring();

        string testFile = Path.Combine(_testDirectory, "test.txt");
        File.WriteAllText(testFile, "initial content");

        Thread.Sleep(100); // Wait for file system events to propagate

        
        // Act
        File.WriteAllText(testFile, "modified content");
        
        Thread.Sleep(100); // Wait for file system events to propagate
        
        // Assert
        _observerMock.Invocations.Should().Contain(i => i.Method.Name == "OnFileChanged" && i.Arguments[0].ToString() == testFile);
    }

    [Fact]
    public void NotifyRenamed_NotifiesObserver()
    {
        // Arrange
        _client.StartMonitoring();

        var oldFilePath = Path.Combine(_testDirectory, "old.txt");
        var newFilePath = Path.Combine(_testDirectory, "new.txt");
        File.WriteAllText(oldFilePath, "test content");

        Thread.Sleep(100); // Wait for file system events to propagate
        
        // Act
        File.Move(oldFilePath, newFilePath);

        Thread.Sleep(100); // Wait for file system events to propagate
        
        // Assert
        _observerMock.Verify(o => o.OnFileRenamed(oldFilePath, newFilePath), Times.Once);
    }

    [Fact]
    public void NotifyDeleted_NotifiesObserver()
    {
        // Arrange
        _client.StartMonitoring();

        var testFile = Path.Combine(_testDirectory, "test.txt");
        File.WriteAllText(testFile, "test content");

        Thread.Sleep(100); // Wait for file system events to propagate

        // Act
        File.Delete(testFile);
        Thread.Sleep(100); // Wait for file system events to propagate

        // Assert
        _observerMock.Verify(o => o.OnDeleted(testFile), Times.Once);
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