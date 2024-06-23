using System.Net.Sockets;
using BoxDropperServer.Strategies;
using DropBoxExercise.Server.Strategies;
using DropBoxExercise.Server.Utils.ClientMessageUtils;

namespace BoxDropperServer.Domain.ClientHandler;

public class ClientHandler : IClientHandler
{
    private readonly string _destinationDirectory;

    public ClientHandler(string destinationDirectory)
    {
        _destinationDirectory = destinationDirectory;
    }

    public void HandleClient(TcpClient client)
    {
        try
        {
            using var networkStream = client.GetStream();
            
            var clientMessage = ClientMessageParser.Parse(networkStream);
            
            ClientMessageValidator.Validate(clientMessage);

            var clientMessageStrategy = ClientMessageStrategyFactory.Build(clientMessage);

            clientMessageStrategy.Validate();
            clientMessageStrategy.Execute(_destinationDirectory);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error handling client: {0}", ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
}
