using ICalculator;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public static class ClassGenerator
{
    public static ICalculator.ICalculator CreateCalculator()
    {
        string code = @"
using task11;
public class Calculator : ICalculator.ICalculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}";

        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var assemblyReferences = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator.ICalculator).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
        };

        var assemblyName = "DynamicCalculatorAssembly_" + Guid.NewGuid();
        var compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            assemblyReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var memoryStreamForAssembly = new MemoryStream();
        var result = compilation.Emit(memoryStreamForAssembly);
        if (!result.Success)
        {
            throw new InvalidOperationException("Compilation failed");
        }
        memoryStreamForAssembly.Seek(0, SeekOrigin.Begin);
        var assembly = AssemblyLoadContext.Default.LoadFromStream(memoryStreamForAssembly);
        var type = assembly.GetType("Calculator");
        if (type == null)
            throw new InvalidOperationException("Type 'Calculator' not found in the compiled assembly.");
        var instance = Activator.CreateInstance(type);
        if (instance is not ICalculator.ICalculator calculator)
            throw new InvalidOperationException("Failed to create an instance of ICalculator.ICalculator.");
        return calculator;
    }
}

