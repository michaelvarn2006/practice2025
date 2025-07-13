using System.Runtime.CompilerServices;
using CommandLib;
using System.Text;
using task07;

namespace FileSystemCommands;

[task07.DisplayName("Directory Size Calculator")]
[task07.Version(1, 0)]
public class DirectorySizeCommand : ICommand
{
    public string? DirectoryName { get; set; }
    public long Size { get; private set; }
    
    [task07.DisplayName("Directory Size Command Constructor")]
    public DirectorySizeCommand(string? DirectoryName)
    {
        this.DirectoryName = DirectoryName;
    }
    
    [task07.DisplayName("Calculate Directory Size")]
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

[task07.DisplayName("File Search Tool")]
[task07.Version(1, 0)]
public class FindFilesCommand : ICommand
{
    public string? DirectoryName { get; set; }
    public string? Mask { get; set; }
    public List<string> Files { get; private set; } = new();
    
    [task07.DisplayName("File Search Command Constructor")]
    public FindFilesCommand(string? DirectoryName, string? Mask)
    {
        this.DirectoryName = DirectoryName;
        this.Mask = Mask;
    }
    
    [task07.DisplayName("Find Files by Pattern")]
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




