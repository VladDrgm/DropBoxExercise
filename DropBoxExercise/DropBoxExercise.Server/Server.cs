using BoxDropperServer.Domain.ClientHandler;
using BoxDropperServer.Domain.TcpListener;

public class Server
{
    private readonly ITcpListener _listener;
    private readonly IClientHandler _clientHandler;
    private bool _isRunning;

    public Server(ITcpListener listener, IClientHandler clientHandler)
    {
        _listener = listener;
        _clientHandler = clientHandler;
    }

    public void RunServer()
    {
        _listener.Start();
        _isRunning = true;
        Console.WriteLine("Server listening...");

        while (_isRunning)
        {

            var client = _listener.AcceptTcpClient();
            Console.WriteLine("Client connected...");
            _clientHandler.HandleClient(client);
        }

        _listener.Stop();
    }

    public void StopServer()
    {
        _isRunning = false;
    }
}
