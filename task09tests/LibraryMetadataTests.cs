using System.Reflection;
using task09;
using Xunit;

namespace task09tests;

public class LibraryMetadataTests
{
    [Fact]
    public void ExtractMetadata_WithValidDll_ShouldOutputCompleteMetadata()
    {
        string currentDir = Directory.GetCurrentDirectory();
        string testDllPath = Path.Combine(currentDir, "..", "..", "..", "..", "FileSystemCommands", "bin", "Debug", "net8.0", "FileSystemCommands.dll");
        
        if (!File.Exists(testDllPath))
        {
            testDllPath = Path.Combine(currentDir, "..", "..", "..", "FileSystemCommands", "bin", "Debug", "net8.0", "FileSystemCommands.dll");
        }
        
        if (!File.Exists(testDllPath))
        {
            throw new FileNotFoundException("Не удалось найти FileSystemCommands.dll для тестирования");
        }
        
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        LibraryMetadata.ExtractMetadata(testDllPath);
        
        string output = consoleOutput.ToString();
        string expectedFileName = Path.GetFileName(testDllPath);
        
        string expectedString = $"Metadata for {expectedFileName} \n" +
            "\n" +
            "Class: DirectorySizeCommand\n" +
            "Display Name: Directory Size Calculator\n" +
            "Version: 1.0\n" +
            "Constructors:\n" +
            "[DisplayName]: Directory Size Command Constructor\n" +
            "  DirectorySizeCommand(String DirectoryName)\n" +
            "Methods:\n" +
            "  get_DirectoryName()\n" +
            "  set_DirectoryName(String value)\n" +
            "  get_Size()\n" +
            "  set_Size(Int64 value)\n" +
            "[DisplayName]: Calculate Directory Size\n" +
            "  Execute()\n" +
            "\n" +
            "Class: FindFilesCommand\n" +
            "Display Name: File Search Tool\n" +
            "Version: 1.0\n" +
            "Constructors:\n" +
            "[DisplayName]: File Search Command Constructor\n" +
            "  FindFilesCommand(String DirectoryName, String Mask)\n" +
            "Methods:\n" +
            "  get_DirectoryName()\n" +
            "  set_DirectoryName(String value)\n" +
            "  get_Mask()\n" +
            "  set_Mask(String value)\n" +
            "  get_Files()\n" +
            "  set_Files(List`1 value)\n" +
            "[DisplayName]: Find Files by Pattern\n" +
            "  Execute()\n" +
            "\n";
        
        Assert.True(output.Contains(expectedString), $"Ожидалось содержимое: {expectedString}\nПолучено: {output}");
        
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }

    [Fact]
    public void ExtractMetadata_WithNonExistentFile_ShouldThrowFileNotFoundException()
    {
        string nonExistentPath = "non_existent_file.dll";
        
        Assert.Throws<FileNotFoundException>(() => LibraryMetadata.ExtractMetadata(nonExistentPath));
    }

    [Fact]
    public void ExtractMetadata_WithInvalidDllFile_ShouldThrowBadImageFormatException()
    {
        string tempPath = Path.GetTempFileName();
        File.WriteAllText(tempPath, "This is not a valid DLL file");
        
        Assert.Throws<BadImageFormatException>(() => LibraryMetadata.ExtractMetadata(tempPath));
        
        File.Delete(tempPath);
    }

    [Fact]
    public void ExtractMetadata_ShouldHandleEmptyAssembly()
    { 
        string currentDir = Directory.GetCurrentDirectory();
        string testDllPath = Path.Combine(currentDir, "..", "..", "..", "..", "FileSystemCommands", "bin", "Debug", "net8.0", "FileSystemCommands.dll");
        
        if (!File.Exists(testDllPath))
        {
            testDllPath = Path.Combine(currentDir, "..", "..", "..", "FileSystemCommands", "bin", "Debug", "net8.0", "FileSystemCommands.dll");
        }
        
        if (!File.Exists(testDllPath))
        {
            throw new FileNotFoundException("Не удалось найти FileSystemCommands.dll для тестирования");
        }
        
        string tempPath = Path.GetTempFileName();
        File.Copy(testDllPath, tempPath, true);
        
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        LibraryMetadata.ExtractMetadata(tempPath);
        
        string output = consoleOutput.ToString();
        Assert.Contains("Metadata for", output);
        
        File.Delete(tempPath);
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
}

