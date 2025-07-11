using Newtonsoft.Json;

namespace task13;

public class Subject
{
    public string? Name { get; set; }
    public int Grade { get; set; }

    public Subject(string? Name, int Grade)
    {
        this.Name = Name;
        this.Grade = Grade;
    }
}

public class Student
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Subject> Grades { get; set; }

    public Student(string? firstName, string? lastName, DateTime birthDate, List<Subject> grades)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.BirthDate = birthDate;
        this.Grades = grades;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            throw new ArgumentException("First name is required");

        if (string.IsNullOrWhiteSpace(LastName))
            throw new ArgumentException("Last name is required");

        if (BirthDate > DateTime.Now)
            throw new ArgumentException("Birth date cannot be in the future");

        if (BirthDate < new DateTime(1900, 1, 1))
            throw new ArgumentException("Invalid birth date");

        if (Grades != null)
        {
            if (Grades.Count == 0)
                throw new ArgumentException("Student must have at least one subject");
            foreach (var subject in Grades)
            {
                if (subject.Grade < 0 || subject.Grade > 100)
                    throw new ArgumentException($"Grade {subject.Grade} for subject '{subject.Name}' is invalid");
                if (string.IsNullOrWhiteSpace(subject.Name))
                    throw new ArgumentException("Subject name must not be null or whitespace");
            }
        }
    }

    public override string ToString()
    {
        return $"{LastName} {FirstName}, born {BirthDate:dd.MM.yyyy}, subjects: {Grades?.Count ?? 0}";
    }
}

public class StudentSerialization
{
    private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        DateFormatString = "dd.MM.yyyy",
        StringEscapeHandling = StringEscapeHandling.Default
    };

    public static void SerializeStudent(Student student, string path)
    {
        string json = JsonConvert.SerializeObject(student, settings);
        File.WriteAllText(path, json);
    }

    public static Student DeserializeStudent(string path)
    {
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<Student>(json, settings) ?? throw new InvalidOperationException("Deserialization returned null");
    }

    static void Main()
    {
        var student = new Student("Michael", "Varnavsky", new DateTime(2006, 06, 13),
            new List<Subject> { new Subject("Summer Practice", 4) });

        string filePath = "Student.json";
        SerializeStudent(student, filePath);

        Student loadedStudent = DeserializeStudent(filePath);

        try
        {
            loadedStudent.Validate();
            Console.WriteLine(loadedStudent);
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"Validation error: {exception.Message}");
        }
        File.Delete(filePath);
    }
}


