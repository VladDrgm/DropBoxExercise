using System.Net.Sockets;
using BoxDropperServer.Domain.ClientHandler;
using BoxDropperServer.Domain.TcpListener;
using FluentAssertions;
using Moq;

namespace DropBoxExercise.Server.Tests.IntegrationTests;

public class ServerIntegrationTests
{
    [Fact]
    public void When_ServerIsRunning_ClientIsAcceptedAndHandled()
    {
        // Arrange
        var mockListener = new Mock<ITcpListener>();
        var mockClientHandler = new Mock<IClientHandler>();
        var mockClient = new Mock<TcpClient>();

        mockListener.Setup(l => l.AcceptTcpClient()).Returns(mockClient.Object);

        var server = new global::Server(mockListener.Object, mockClientHandler.Object);

        // Act
        var serverTask = Task.Run(() => server.RunServer());
        Task.Delay(1000).Wait(); // Let the server start and accept client

        // Assert
        mockListener.Invocations.Should().Contain(i => i.Method.Name == nameof(mockListener.Object.Start));
        mockListener.Invocations.Should().Contain(i => i.Method.Name == nameof(mockListener.Object.AcceptTcpClient));
        mockClientHandler.Invocations.Should().Contain(i => i.Method.Name == nameof(mockClientHandler.Object.HandleClient));

        // Cleanup
        server.StopServer();
        serverTask.Wait(); // Ensure the server task completes
    }
}