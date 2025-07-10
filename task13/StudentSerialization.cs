using Newtonsoft.Json;

namespace task13;

public class Subject
{
    public string Name { get; set; }
    public int Grade { get; set; }

    public Subject(string Name, int Grade)
    {
        this.Name = Name;
        this.Grade = Grade;
    }
}

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Subject> Grades { get; set; }

    public Student(string FirstName, string LastName, DateTime BirthDate, List<Subject> Grades)
    {
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.BirthDate = BirthDate;
        this.Grades = Grades;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            throw new ArgumentException("Имя обязательно");

        if (string.IsNullOrWhiteSpace(LastName))
            throw new ArgumentException("Фамилия обязательна");

        if (BirthDate > DateTime.Now)
            throw new ArgumentException("Дата рождения не может быть в будущем");

        if (BirthDate < new DateTime(1900, 1, 1))
            throw new ArgumentException("Некорректная дата рождения");

        if (Grades != null)
        {
            foreach (var subject in Grades)
            {
                if (subject.Grade < 0 || subject.Grade > 100)
                    throw new ArgumentException($"Оценка {subject.Grade} по предмету '{subject.Name}' недопустима");
                if (subject.Name == null)
                    throw new ArgumentException("Название предмета не должно быть null");
            }
        }
    }

    public override string ToString()
    {
        return $"{LastName} {FirstName}, родился {BirthDate:dd.MM.yyyy}, предметов: {Grades?.Count ?? 0}";
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
        return JsonConvert.DeserializeObject<Student>(json, settings) ?? throw new InvalidOperationException("Десериализация вернула null");
    }

    static void Main()
    {
        var student = new Student("Михаил", "Варнавский", new DateTime(2006, 06, 13),
            new List<Subject> { new Subject("Летняя практика", 4) });

        string filePath = "Student.json";
        SerializeStudent(student, filePath);

        Student loadedStudent = DeserializeStudent(filePath);

        try
        {
            loadedStudent.Validate();
            Console.WriteLine(loadedStudent);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка валидации: {ex.Message}");
        }
        File.Delete(filePath);
    }
}


