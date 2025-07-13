using System.IO;
using PluginLoader;
using Xunit;

namespace PluginLoaderTests;

public class PluginLoaderTests
{
    [Fact]
    public void PluginLoader_ShouldExecuteAllPlugins_InCorrectOrder()
    {
        var parent = Directory.GetParent(Directory.GetCurrentDirectory());
        parent = parent?.Parent;
        parent = parent?.Parent;
        parent = parent?.Parent;
        if (parent == null)
            throw new InvalidOperationException("Failed to get path to PluginsBinaries folder");
        var pluginDirectory = Path.Combine(parent.FullName, "PluginsBinaries");
        var pluginLoader = new PluginLoader.PluginLoader();

        var consoleOutput = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(consoleOutput);

        pluginLoader.LoadAndExecutePlugins(pluginDirectory);

        var output = consoleOutput.ToString();

        int idx1 = output.IndexOf("First plugin is executing");
        int idx2 = output.IndexOf("Second plugin is executing");
        int idx3 = output.IndexOf("Third plugin is executing");
        int idx4 = output.IndexOf("Fourth plugin is executing");

        Assert.True(idx1 >= 0, "First plugin output not found");
        Assert.True(idx3 > idx1, "Third plugin should come after first");
        Assert.True(idx2 > idx3, "Second plugin should come after third");
        Assert.True(idx4 > idx2, "Fourth plugin should come after second");

        Console.SetOut(originalOutput);
    }
    
    [Fact]
    public void PluginLoader_ShouldNotOutputAnything_WhenNoPluginsInDirectory()
    {
        var parent = Directory.GetParent(Directory.GetCurrentDirectory());
        parent = parent?.Parent;
        parent = parent?.Parent;
        parent = parent?.Parent;
        if (parent == null)
            throw new InvalidOperationException("Failed to get path to EmptyDir folder");
        var emptyDir = Path.Combine(parent.FullName, "EmptyDir");
        var pluginLoader = new PluginLoader.PluginLoader();

        var consoleOutput = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(consoleOutput);

        pluginLoader.LoadAndExecutePlugins(emptyDir);

        var output = consoleOutput.ToString();
        Assert.True(string.IsNullOrWhiteSpace(output));

        Console.SetOut(originalOutput);
    }
}

