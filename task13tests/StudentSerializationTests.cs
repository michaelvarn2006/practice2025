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

        var student = new Student("Иван", "Иванов", new DateTime(2000, 1, 1),
            new List<Subject> { new Subject("Математика", 5) });

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
    }
    [Fact]
    public void Validate_ShouldThrowWhenFirstNameIsEmpty()
    {
        var invalidStudent = new Student(
            "",
            "Иванов",
            new DateTime(2000, 1, 1),
            new List<Subject> { new Subject("Математика", 5) }
        );

        var ex = Assert.Throws<ArgumentException>(() => invalidStudent.Validate());
        Assert.Contains("Имя обязательно", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenLastNameIsNull()
    {
        var invalidStudent = new Student(
            "Иван",
            null,
            new DateTime(2000, 1, 1),
            new List<Subject> { new Subject("Математика", 5) }
        );

        var ex = Assert.Throws<ArgumentException>(() => invalidStudent.Validate());
        Assert.Contains("Фамилия обязательна", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenBirthDateInFuture()
    {
        var invalidStudent = new Student(
            "Иван",
            "Иванов",
            DateTime.Now.AddDays(1),
            new List<Subject> { new Subject("Математика", 5) }
        );

        var ex = Assert.Throws<ArgumentException>(() => invalidStudent.Validate());
        Assert.Contains("не может быть в будущем", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenBirthDateTooOld()
    {
        var invalidStudent = new Student(
            "Иван",
            "Иванов",
            new DateTime(1899, 12, 31),
            new List<Subject> { new Subject("Математика", 5) }
        );

        var ex = Assert.Throws<ArgumentException>(() => invalidStudent.Validate());
        Assert.Contains("Некорректная дата рождения", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenGradeTooLow()
    {
        var invalidStudent = new Student(
            "Иван",
            "Иванов",
            new DateTime(2000, 1, 1),
            new List<Subject> { new Subject("Математика", -1) }
        );

        var ex = Assert.Throws<ArgumentException>(() => invalidStudent.Validate());
        Assert.Contains("недопустима", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenGradeTooHigh()
    {
        var invalidStudent = new Student(
            "Иван",
            "Иванов",
            new DateTime(2000, 1, 1),
            new List<Subject> { new Subject("Математика", 101) }
        );

        var ex = Assert.Throws<ArgumentException>(() => invalidStudent.Validate());
        Assert.Contains("недопустима", ex.Message);
    }

    [Fact]
    public void Validate_ShouldThrowWhenSubjectNameIsNull()
    {
        var invalidStudent = new Student(
            "Иван",
            "Иванов",
            new DateTime(2000, 1, 1),
            new List<Subject> { new Subject(null, 5) }
        );

        var ex = Assert.Throws<ArgumentException>(() => invalidStudent.Validate());
        Assert.Contains("Название предмета", ex.Message);
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

        var ex = Assert.Throws<InvalidOperationException>(() =>
            StudentSerialization.DeserializeStudent(path));
        Assert.Contains("Десериализация вернула null", ex.Message);

        File.Delete(path);
    }
}

