using System.Net.Sockets;

namespace BoxDropperServer.Domain.TcpListener;

public interface ITcpListener
{
    void Start();
    TcpClient AcceptTcpClient();
    void Stop();
}