namespace DropBoxExercise.Server.Strategies;

public interface IClientMessageStrategy
{
    void Execute(string destinationDirectoryPath);

    void Validate();
}