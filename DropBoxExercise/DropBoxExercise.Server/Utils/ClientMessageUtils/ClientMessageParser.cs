using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using DropBoxExercise.Server.Dto;

namespace DropBoxExercise.Server.Utils.ClientMessageUtils;

public static class ClientMessageParser
{
    public static ClientMessageDto Parse(NetworkStream stream)
    {
        var messageLength = GetMessageLength(stream);
        var messageBytes = GetMessageBytes(stream, messageLength);
        
        var jsonMessage = GetMessageString(messageBytes);
        
        return JsonSerializer.Deserialize<ClientMessageDto>(jsonMessage)!;
    }

    private static int GetMessageLength(NetworkStream networkStream)
    {
        var messageLengthBytes = new byte[4];
        var readBytes = networkStream.Read(messageLengthBytes, 0, messageLengthBytes.Length);
        
        if (readBytes != messageLengthBytes.Length)
        {
            throw new Exception("Failed to read message length");
        }
        
        return BitConverter.ToInt32(messageLengthBytes, 0);
    }
    
    private static byte[] GetMessageBytes(NetworkStream networkStream, int messageLength)
    {
        var messageBytes = new byte[messageLength];
        var readBytes = networkStream.Read(messageBytes, 0, messageBytes.Length);
        
        if (readBytes != messageBytes.Length)
        {
            throw new Exception("Failed to read message");
        }
        return messageBytes;
    }
    
    private static string GetMessageString(byte[] messageBytes) => Encoding.UTF8.GetString(messageBytes);
}