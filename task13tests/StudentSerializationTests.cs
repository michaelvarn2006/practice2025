using Xunit;
using task13;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace task13tests;

public class UnitTest1
{
    [Fact]
    public void SerializeAndDeserialize_ShouldReturnEquivalentStudent()
    {
        var student = new Student("Michael", "Varnavsky", new DateTime(2006, 6, 13),
            new List<Subject> { new Subject("Summer Practice", 4) });

        var path = "test_student.json";
        StudentSerialization.SerializeStudent(student, path);
        var deserialized = StudentSerialization.DeserializeStudent(path);

        deserialized.Validate();
        Assert.Equal(student.FirstName, deserialized.FirstName);
        Assert.Equal(student.LastName, deserialized.LastName);
        Assert.Equal(student.BirthDate, deserialized.BirthDate);
        Assert.Equal(student.Grades.Count, deserialized.Grades.Count);
        Assert.Equal(student.Grades[0].Name, deserialized.Grades[0].Name);
        Assert.Equal(student.Grades[0].Grade, deserialized.Grades[0].Grade);

        File.Delete(path);
    }
    [Fact]
    public void Validate_ShouldThrowWhenFirstNameIsEmpty()
    {
        var student = new Student(
            "",
            "Varnavsky",
            new DateTime(2006, 6, 13),
            new List<Subject> { new Subject("Summer Practice", 4) }
        );

        var exception = Assert.Throws<ArgumentException>(() => student.Validate());
        Assert.Contains("First name is required", exception.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenLastNameIsNull()
    {
        var student = new Student(
            "Michael",
            null,
            new DateTime(2006, 6, 13),
            new List<Subject> { new Subject("Summer Practice", 4) }
        );

        var exception = Assert.Throws<ArgumentException>(() => student.Validate());
        Assert.Contains("Last name is required", exception.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenBirthDateInFuture()
    {
        var student = new Student(
            "Michael",
            "Varnavsky",
            DateTime.Now.AddDays(1),
            new List<Subject> { new Subject("Summer Practice", 4) }
        );

        var exception = Assert.Throws<ArgumentException>(() => student.Validate());
        Assert.Contains("Birth date cannot be in the future", exception.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenBirthDateTooOld()
    {
        var student = new Student(
            "Michael",
            "Varnavsky",
            new DateTime(1899, 12, 31),
            new List<Subject> { new Subject("Summer Practice", 4) }
        );

        var exception = Assert.Throws<ArgumentException>(() => student.Validate());
        Assert.Contains("Invalid birth date", exception.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenGradeTooLow()
    {
        var student = new Student(
            "Michael",
            "Varnavsky",
            new DateTime(2006, 6, 13),
            new List<Subject> { new Subject("Summer Practice", -1) }
        );

        var exception = Assert.Throws<ArgumentException>(() => student.Validate());
        Assert.Contains($"Grade -1 for subject '{student.Grades[0].Name}' is invalid", exception.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenGradeTooHigh()
    {
        var student = new Student(
            "Michael",
            "Varnavsky",
            new DateTime(2006, 6, 13),
            new List<Subject> { new Subject("Summer Practice", 101) }
        );

        var exception = Assert.Throws<ArgumentException>(() => student.Validate());
        Assert.Contains($"Grade 101 for subject '{student.Grades[0].Name}' is invalid", exception.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenSubjectNameIsNull()
    {
        var student = new Student(
            "Michael",
            "Varnavsky",
            new DateTime(2006, 6, 13),
            new List<Subject> { new Subject(null, 4) }
        );

        var exception = Assert.Throws<ArgumentException>(() => student.Validate());
        Assert.Contains("Subject name must not be null or whitespace", exception.Message);
    }

    [Fact]
    public void DeserializeStudent_ShouldThrowWhenFileNotFound()
    {
        Assert.Throws<FileNotFoundException>(() =>
            StudentSerialization.DeserializeStudent("nonexistent.json"));
    }

    [Fact]
    public void DeserializeStudent_ShouldThrowWhenInvalidJson()
    {
        var path = "invalid.json";
        File.WriteAllText(path, "{this is not valid json}");

        Assert.Throws<Newtonsoft.Json.JsonReaderException>(() =>
            StudentSerialization.DeserializeStudent(path));

        File.Delete(path);
    }

    [Fact]
    public void DeserializeStudent_ShouldThrowWhenNullReturned()
    {
        var path = "null.json";
        File.WriteAllText(path, "null");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            StudentSerialization.DeserializeStudent(path));
        Assert.Contains("Deserialization returned null", exception.Message);

        File.Delete(path);
    }
}


