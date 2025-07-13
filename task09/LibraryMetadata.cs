using System.Reflection;
using task07;

namespace task09;

public class LibraryMetadata
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run --project task09 -- <path-to-dll>");
            return;
        }

        string dllPath = args[0];
        
        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Error: File '{dllPath}' not found.");
            return;
        }

        try
        {
            ExtractMetadata(dllPath);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error extracting metadata: {exception.Message}");
        }
    }

    public static void ExtractMetadata(string dllPath)
    {
        Console.WriteLine($"Metadata for {Path.GetFileName(dllPath)} \n");
        
        Assembly assembly = Assembly.LoadFrom(dllPath);
        Type[] types = assembly.GetTypes();

        foreach (Type type in types)
        {
            if (type.IsClass && !type.IsAbstract)
            {
                ReflectionHelper.PrintTypeInfo(type);
            }
        }
    }
}
