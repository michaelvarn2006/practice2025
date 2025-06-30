using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using task05;
using Xunit;
namespace task05tests;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() { }
    public string SumString(string a, string b) => new string(a + b);
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
        Assert.Contains("SumString", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
        Assert.Contains("PublicField", fields);
    }
    [Fact]
    public void GetMethodParams_ReturnsCorrectParamsEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var param = analyzer.GetMethodParams("Method");
        Assert.Contains("Void", param);
    }
    [Fact]
    public void GetMethodParams_ReturnsCorrectParamsNotEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var param = analyzer.GetMethodParams("SumString");
        Assert.Contains("String", param);
        Assert.Contains("a", param);
        Assert.Contains("b", param);
    }
    [Fact]
    public void GetPropertries_ReturnCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();
        Assert.Contains("Property", properties);
    }
    [Fact]
    public void HasAttribute_ReturnCorrectBoolean()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }
}
