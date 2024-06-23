using System.Net.Sockets;

namespace BoxDropperServer.Domain.ClientHandler;

public interface IClientHandler
{
    void HandleClient(TcpClient client);
}