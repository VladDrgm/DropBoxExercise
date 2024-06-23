using System.Net;
using System.Net.Sockets;
using TcpListenerAlias = System.Net.Sockets.TcpListener;

namespace BoxDropperServer.Domain.TcpListener;

public class TcpListenerWrapper : ITcpListener
{
    private readonly TcpListenerAlias _tcpListener;

    public TcpListenerWrapper(IPAddress address, int port)
    {
        _tcpListener = new TcpListenerAlias(address, port);
    }

    public void Start() => _tcpListener.Start();
    public TcpClient AcceptTcpClient() => _tcpListener.AcceptTcpClient();
    public void Stop() => _tcpListener.Stop();
}

