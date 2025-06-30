using System;
using System.Reflection;
using System.Collections.Generic;
namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }
    public IEnumerable<string> GetPublicMethods() => _type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
        .Select(m => m.Name);
    public IEnumerable<string> GetMethodParams(string methodname) => _type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Where(m => m.Name == methodname)
        .SelectMany(m => new[] {
            m.ReturnType.Name
        }.Concat(m.GetParameters()
            .Select(p => p.Name)
            ));
    public IEnumerable<string> GetAllFields() => _type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
        .Select(p => p.Name);
    public IEnumerable<string> GetProperties() => _type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
        .Select(p => p.Name);
    public bool HasAttribute<T>() where T : Attribute => _type.IsDefined(typeof(T));
}

