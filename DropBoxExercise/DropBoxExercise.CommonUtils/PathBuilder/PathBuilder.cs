namespace DropBoxExercise.CommonUtils.PathBuilder;

public static class PathBuilder
{
    public static string BuildFilePath(string originPath, string destinationDirectoryPath)
    {
        var fullPath = destinationDirectoryPath;

        var originPathDirectories = originPath.Split("/");
        
        var targetPathDirectories = destinationDirectoryPath.Split("/");
        
        var fileIsInSubdirectory = originPathDirectories.Length > targetPathDirectories.Length;
        
        if (fileIsInSubdirectory)
        {
            var subdirectoryDepth = originPathDirectories.Length - targetPathDirectories.Length;
            
            Console.WriteLine("Subdirectory depth : {0}", subdirectoryDepth);
            
            for ( int i = 0; i < subdirectoryDepth; i++)
            {
                fullPath = Path.Combine(fullPath, originPathDirectories[targetPathDirectories.Length + i]);
                Console.WriteLine("Full path is in loop: {0}", fullPath);
            }
        }
        
        return fullPath;
    }
}