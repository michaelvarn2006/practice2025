namespace task07;

using System.ComponentModel;
using System.Dynamic;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property)]
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
        var dispclassattr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (dispclassattr != null)
        {
            Console.WriteLine(dispclassattr.DisplayName);
        }
        var versclassattr = type.GetCustomAttribute<VersionAttribute>();
        if (versclassattr != null)
        {
            Console.WriteLine($"Версия: {versclassattr.Major}.{versclassattr.Minor}");
        }
        Console.WriteLine("Методы");
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
        {
            var dispmethodattr = method.GetCustomAttribute<DisplayNameAttribute>();
            if (dispmethodattr != null)
            {
                Console.WriteLine(dispmethodattr.DisplayName);
            }
        }
        Console.WriteLine("Свойства");
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
        {
            var disppropattr = prop.GetCustomAttribute<DisplayNameAttribute>();
            if (disppropattr != null)
            {
                Console.WriteLine(disppropattr.DisplayName);
            }
        }
    }
}
