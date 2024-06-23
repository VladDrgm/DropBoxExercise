namespace DropBoxExercise.Server.Strategies;

public interface IClientMessageStrategy
{
    void Execute(string destinationDirectory);

    void Validate();
}