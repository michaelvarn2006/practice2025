using System.Runtime.CompilerServices;
using CommandLib;
using System.Text;
namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    string? DirectoryName { get; set; }
    public long Size { get; private set; }
    public DirectorySizeCommand(string? DirectoryName)
    {
        this.DirectoryName = DirectoryName;
    }
    public void Execute()
    {
        if (DirectoryName == null)
            throw new ArgumentNullException(nameof(DirectoryName));
        if (string.IsNullOrWhiteSpace(DirectoryName))
            throw new ArgumentException("DirectoryName must not be empty or whitespace.", nameof(DirectoryName));
        if (!Directory.Exists(DirectoryName))
            throw new DirectoryNotFoundException($"The directory '{DirectoryName}' does not exist.");
        DirectoryInfo info = new DirectoryInfo(DirectoryName);
        Size = info.EnumerateFiles().Select(f => f.Length).Sum();
    }
}

public class FindFilesCommand : ICommand
{
    string? DirectoryName { get; set; }
    string? Mask { get; set; }
    public List<string> Files { get; private set; } = new();
    public FindFilesCommand(string? DirectoryName, string? Mask)
    {
        this.DirectoryName = DirectoryName;
        this.Mask = Mask;
    }
    public void Execute()
    {
        if (DirectoryName == null)
            throw new ArgumentNullException(nameof(DirectoryName));
        if (Mask == null)
            throw new ArgumentNullException(nameof(Mask));
        if (string.IsNullOrWhiteSpace(DirectoryName))
            throw new ArgumentException("DirectoryName must not be empty or whitespace.", nameof(DirectoryName));
        if (string.IsNullOrWhiteSpace(Mask))
            throw new ArgumentException("Mask must not be empty or whitespace.", nameof(Mask));
        if (!Directory.Exists(DirectoryName))
            throw new DirectoryNotFoundException($"The directory '{DirectoryName}' does not exist.");
        Files = Directory.GetFiles(DirectoryName, Mask, SearchOption.AllDirectories).ToList();
    }
}

