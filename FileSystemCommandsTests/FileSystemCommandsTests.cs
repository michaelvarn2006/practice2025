using Xunit;
using CommandRunner;
using FileSystemCommands;
namespace FileSystemCommandsTests;

public class UnitTest1
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        string? baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        if (baseDirectory == null) throw new DirectoryNotFoundException("Couldn't find base directory!\n");
        var testDirectory = Path.Combine(baseDirectory, "TestDir");
        Directory.CreateDirectory(testDirectory);
        File.WriteAllText(Path.Combine(testDirectory, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDirectory, "test2.txt"), "World");
        var testedCommand = new DirectorySizeCommand(testDirectory);
        testedCommand.Execute();
        Directory.Delete(testDirectory, true);
        Assert.True(testedCommand.Size == 10);
    }
    [Fact]
    public void DirectorySizeCommand_ShouldThrowException_WhenDirectoryDoesNotExist()
    {
        string? baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        if (baseDirectory == null) throw new DirectoryNotFoundException("Couldn't find base directory!\n");
        var nonExistentDirectory = Path.Combine(baseDirectory, "NonExistentDir");
        var testedCommand = new DirectorySizeCommand(nonExistentDirectory);
        Assert.Throws<DirectoryNotFoundException>(() => testedCommand.Execute());
    }
    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        string? baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        if (baseDirectory == null) throw new DirectoryNotFoundException("Couldn't find base directory!\n");
        var testDirectory = Path.Combine(baseDirectory, "TestDir");
        Directory.CreateDirectory(testDirectory);
        File.WriteAllText(Path.Combine(testDirectory, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDirectory, "file2.log"), "Log");
        var testedCommand = new FindFilesCommand(testDirectory, "*.txt");
        testedCommand.Execute();
        Directory.Delete(testDirectory, true);
        Assert.Single(testedCommand.Files);
        Assert.Contains(Path.Combine(testDirectory, "file1.txt"), testedCommand.Files);
    }
    [Fact]
    public void FindFilesCommand_ShouldThrowException_WhenDirectoryDoesNotExist()
    {
        string? baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        if (baseDirectory == null) throw new DirectoryNotFoundException("Couldn't find base directory!\n");
        var nonExistentDirectory = Path.Combine(baseDirectory, "NonExistentDir");
        var testedCommand = new FindFilesCommand(nonExistentDirectory, "*.txt");
        Assert.Throws<DirectoryNotFoundException>(() => testedCommand.Execute());
    }
    [Fact]
    public void CommandRunnerMain_ShouldRunCommandsSuccessfully()
    {
        string baseDirectory = Directory.GetCurrentDirectory();
        var output = new StringWriter();
        Console.SetOut(output);
        CommandRunner.CommandRunner.Main();
        Assert.Contains("10", output.ToString());
        Assert.Contains("/home/michael/practice20252/practice2025/TestDir/test1.txt", output.ToString());
        Assert.Contains("/home/michael/practice20252/practice2025/TestDir/test2.txt", output.ToString());
    }
    [Fact]
    public void DirectorySizeCommand_ShouldThrowArgumentNullException_WhenDirectoryNameIsNull()
    {
        var testedCommand = new DirectorySizeCommand(null);
        Assert.Throws<ArgumentNullException>(() => testedCommand.Execute());
    }
    [Fact]
    public void DirectorySizeCommand_ShouldThrowArgumentException_WhenDirectoryNameIsEmpty()
    {
        var testedCommand = new DirectorySizeCommand("");
        Assert.Throws<ArgumentException>(() => testedCommand.Execute());
    }
    [Fact]
    public void FindFilesCommand_ShouldThrowArgumentNullException_WhenDirectoryNameIsNull()
    {
        var testedCommand = new FindFilesCommand(null, "*.txt");
        Assert.Throws<ArgumentNullException>(() => testedCommand.Execute());
    }
    [Fact]
    public void FindFilesCommand_ShouldThrowArgumentNullException_WhenMaskIsNull()
    {
        var testedCommand = new FindFilesCommand("/tmp", null);
        Assert.Throws<ArgumentNullException>(() => testedCommand.Execute());
    }
    [Fact]
    public void FindFilesCommand_ShouldThrowArgumentException_WhenDirectoryNameIsEmpty()
    {
        var testedCommand = new FindFilesCommand("", "*.txt");
        Assert.Throws<ArgumentException>(() => testedCommand.Execute());
    }
    [Fact]
    public void FindFilesCommand_ShouldThrowArgumentException_WhenMaskIsEmpty()
    {
        var testedCommand = new FindFilesCommand("/tmp", "");
        Assert.Throws<ArgumentException>(() => testedCommand.Execute());
    }
    [Fact]
    public void FindFilesCommand_ShouldReturnEmptyList_WhenNoFilesFound()
    {
        string? baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        if (baseDirectory == null) throw new DirectoryNotFoundException("Couldn't find base directory!\n");
        var testDirectory = Path.Combine(baseDirectory, "TestDir");
        Directory.CreateDirectory(testDirectory);
        var testedCommand = new FindFilesCommand(testDirectory, "*.notfound");
        testedCommand.Execute();
        Directory.Delete(testDirectory, true);
        Assert.Empty(testedCommand.Files);
    }
}

