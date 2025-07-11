namespace CommandRunner;

using System.Reflection;

public class CommandRunner
{
    public static void Main()
    {
        string? baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        if (baseDirectory == null)
            throw new DirectoryNotFoundException("Couldn't find base directory!\n");
        string? dllPath = Directory.GetFiles(baseDirectory, "FileSystemCommands.dll", SearchOption.AllDirectories).FirstOrDefault();
        if (dllPath == null)
            throw new FileNotFoundException("Couldn't find dynamic library!\n");
        string testDirectory = Path.Combine(baseDirectory, "TestDir");
        Directory.CreateDirectory(testDirectory);
        string firstFilePath = Path.Combine(testDirectory, "test1.txt");
        string secondFilePath = Path.Combine(testDirectory, "test2.txt");
        string fileMask = "*.txt";
        File.WriteAllText(firstFilePath, "Hello");
        File.WriteAllText(secondFilePath, "World");
        Assembly loadedAssembly = Assembly.LoadFrom(dllPath);
        Type? directorySizeCommandType = loadedAssembly.GetType("FileSystemCommands.DirectorySizeCommand");
        if (directorySizeCommandType == null)
            throw new TypeLoadException("Couldn't load type 'FileSystemCommands.DirectorySizeCommand'.");
        object? directorySizeCommandInstance = Activator.CreateInstance(directorySizeCommandType, new object[] { testDirectory });
        directorySizeCommandInstance?.GetType().GetMethod("Execute")?.Invoke(directorySizeCommandInstance, null);
        var directorySize = directorySizeCommandInstance?.GetType().GetProperty("Size")?.GetValue(directorySizeCommandInstance, null);
        if (directorySize != null)
            Console.WriteLine(directorySize);
        List<string>? foundFiles = null;
        Type? findFilesCommandType = loadedAssembly.GetType("FileSystemCommands.FindFilesCommand");
        if (findFilesCommandType == null)
            throw new TypeLoadException("Couldn't load type 'FileSystemCommands.FindFilesCommand'.");
        object? findFilesCommandInstance = Activator.CreateInstance(findFilesCommandType, new object[] { testDirectory, fileMask });
        findFilesCommandInstance?.GetType().GetMethod("Execute")?.Invoke(findFilesCommandInstance, null);
        foundFiles = findFilesCommandInstance?.GetType().GetProperty("Files")?.GetValue(findFilesCommandInstance, null) as List<string>;
        if (foundFiles != null)
            foundFiles.ForEach(f => Console.WriteLine(f));
        Directory.Delete(testDirectory, true);
    }
}

