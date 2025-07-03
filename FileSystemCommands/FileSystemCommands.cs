using System.Runtime.CompilerServices;
using CommandLib;
namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    string? DirectoryName { get; set; }
    public uint Size{ get; private set; }
    public DirectorySizeCommand(string? DirectoryName)
    {
        this.DirectoryName = DirectoryName;
    }
    public void Execute()
    {
        
    }
}

public class FindFilesCommand : ICommand
{
    string? DirectoryName { get; set; }
    string? Mask { get; set; }
    public uint Size { get; private set; }
    public FindFilesCommand(string? DirectoryName, string? Mask)
    {
        this.DirectoryName = DirectoryName;
        this.Mask = Mask;
    }
    public void Execute()
    {

    }
}

