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
            
            for ( int i = 0; i < subdirectoryDepth; i++)
            {
                fullPath = Path.Combine(fullPath, originPathDirectories[targetPathDirectories.Length + i]);
            }
        }
        
        return fullPath;
    }
}