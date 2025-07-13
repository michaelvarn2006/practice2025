namespace task07;
using System.Dynamic;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Constructor)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; set; }
    public DisplayNameAttribute(string DisplayName)
    {
        this.DisplayName = DisplayName;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public VersionAttribute(int Major, int Minor)
    {
        this.Major = Major;
        this.Minor = Minor;
    }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }
    [DisplayName("Тестовый метод")]
    public void TestMethod()
    {

    }
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        if (IsCompilerGenerated(type))
            return;
            
        Console.WriteLine($"Class: {type.Name}");
        
        var dispclassattr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (dispclassattr != null)
        {
            Console.WriteLine($"Display Name: {dispclassattr.DisplayName}");
        }
        var versclassattr = type.GetCustomAttribute<VersionAttribute>();
        if (versclassattr != null)
        {
            Console.WriteLine($"Version: {versclassattr.Major}.{versclassattr.Minor}");
        }
        
        var constructors = type.GetConstructors();
        if (constructors.Any())
        {
            Console.WriteLine("Constructors:");
            foreach (var ctor in constructors)
            {
                var dispctorattr = ctor.GetCustomAttribute<DisplayNameAttribute>();
                if (dispctorattr != null)
                {
                    Console.WriteLine($"[DisplayName]: {dispctorattr.DisplayName}");
                }
                var parameters = ctor.GetParameters();
                var paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"  {type.Name}({paramList})");
            }
        }
        
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                         .Where(m => !IsCompilerGenerated(m));
        if (methods.Any())
        {
            Console.WriteLine("Methods:");
            foreach (var method in methods)
            {
                var dispmethodattr = method.GetCustomAttribute<DisplayNameAttribute>();
                if (dispmethodattr != null)
                {
                    Console.WriteLine($"[DisplayName]: {dispmethodattr.DisplayName}");
                }
                var parameters = method.GetParameters();
                var paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"  {method.Name}({paramList})");
            }
        }
        
        Console.WriteLine();
    }

    private static bool IsCompilerGenerated(Type type)
    {
        return type.Name.StartsWith("<");
    }

    private static bool IsCompilerGenerated(MethodInfo method)
    {
        return method.Name.StartsWith("<");
    }
}
