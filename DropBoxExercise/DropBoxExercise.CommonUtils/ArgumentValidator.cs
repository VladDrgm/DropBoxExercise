namespace DropBoxExercise.CommonUtils;

public static class ArgumentValidator
{
    public static void Validate(string[] args)
    {
        ValidateArgs(args);
        ValidatePath(args[0]);
    }
    
    
    private static void ValidateArgs(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Error! Please insert a <directory path> as an argument.");
            Environment.Exit(1);
        }
    }

    private static void ValidatePath(string path)
    {
        if (!Directory.Exists(path))
        {
            Console.Error.WriteLine("Error! The specified directory does not exist.");
            Environment.Exit(1);
        }
    }
}