namespace BoxDropperServer.Utils;

public static class DirectoryManager
{
    public static void SaveFile(string path, byte[] data)
    {
        
        Console.WriteLine("Saving File in Directory: " + path);
        
        File.WriteAllBytes(path, data);
    }
    
    public static void DeleteFile(string path)
    {
        // if (fileName is null)
        // {
        //     Console.WriteLine("File being deleted....");
        //     // var filesInDirectory = Directory.GetFiles(directoryPath);
        //     // foreach (var file in filesInDirectory)
        //     // {
        //     //     File.Delete(file);
        //     // }
        //     
        //     var isFile = File.Exists(directoryPath);
        //     Console.WriteLine("Is file: " + isFile);
        //     
        //     File.Delete(directoryPath);
        // }
        // else
        // {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        // }
    }
    
    public static void CreateDirectory(string fullPath)
    {
        Console.WriteLine("Test create directory; Directory full path: " + fullPath);
        try
        {
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                Console.WriteLine($"Created directory: {fullPath}");
            }
            else
            {
                Console.WriteLine($"Directory already exists: {fullPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating directory '{fullPath}': {ex.Message}");
            throw;
        }
    }
    
    public static void DeleteDirectory(string fullPath)
    {
        try
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true); // Delete recursively
                Console.WriteLine($"Deleted folder: {fullPath}");
            }
            else
            {
                Console.WriteLine($"Folder does not exist: {fullPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting folder '{fullPath}': {ex.Message}");
            throw; // Rethrow the exception to propagate it
        }
    }
}