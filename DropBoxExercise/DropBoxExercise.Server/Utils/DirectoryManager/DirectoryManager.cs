namespace BoxDropperServer.Utils;

public static class DirectoryManager
{
    public static void SaveFile(string path, byte[] data)
    {
        File.WriteAllBytes(path, data);
    }
    
    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    
    public static void CreateDirectory(string fullPath)
    {
        try
        {
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            else
            {
                Console.WriteLine($"Directory already exists: {fullPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating directory '{fullPath}': {ex.Message}");
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
            }
            else
            {
                Console.WriteLine($"Folder does not exist: {fullPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error deleting folder '{fullPath}': {ex.Message}");
            throw;
        }
    }
}