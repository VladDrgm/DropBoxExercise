using System.Text;
using System.Text.Json;
using DropBoxExercise.Client.Dto;

namespace DropBoxExercise.Client.Utils;

public static class ServerMessageParser
{
    public static byte[] ParseMessage(ServerMessageDto message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(jsonMessage);
        return messageBytes;
    }
}