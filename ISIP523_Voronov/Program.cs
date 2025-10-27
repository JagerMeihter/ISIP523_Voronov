
using System.Reflection;

List<string> StudentNamesRegistry = new List<string>();
List<Student> StudentRoster = new List<Student>();
List<Teacher> FacultyMembers = new List<Teacher>();
List<Course> CourseCatalog = new List<Course>();

bool programRunning = true;
while (programRunning)
{
    Console.WriteLine("----------ГЛАВНОЕ МЕНЮ----------");
    Console.WriteLine("1 - Зарегистрировать студента");
    Console.WriteLine("2 - Показать список студентов");
    Console.WriteLine("3 - Добавить преподавателя");
    Console.WriteLine("4 - Показать список преподавателей");
    Console.WriteLine("5 - Создать новый курс");
    Console.WriteLine("6 - Показать все курсы");
    Console.WriteLine("7 - Найти курсы по имени студента");
    Console.WriteLine("8 - Показать полную базу данных");
    Console.WriteLine("0 - Завершить программу");

    int userChoice = Convert.ToInt32(Console.ReadLine());

    switch (userChoice)

    {
        case 3:
            Console.WriteLine("Введите идентификатор: ");
            int teacherId = Convert.ToInt32(Console.ReadLine());
            if (teacherId == 0)
            {
                Console.WriteLine("Идентификатор не может быть пустым");
                break;
            }
            Console.WriteLine("Введите полное имя: ");
            string teacherFullName = Console.ReadLine();
            if (string.IsNullOrEmpty(teacherFullName))
            {
                Console.WriteLine("Имя не может быть пустым");
                break;
            }
            Console.WriteLine("Введите дату рождения: (ГГГГ,ММ,ДД)");
            DateOnly teacherBirthDate = DateOnly.Parse(Console.ReadLine());
            bool teacherDateValid = DateOnly.TryParse(Console.ReadLine(), out teacherBirthDate);
            if (!teacherDateValid)
            {
                Console.WriteLine("Неверный формат даты");
                break;
            }
            Console.WriteLine("Введите пол: ");
            string teacherGender = Console.ReadLine();
            if (string.IsNullOrEmpty(teacherGender))
            {
                Console.WriteLine("Пол не может быть пустым");
                break;
            }
            Console.WriteLine("Введите стаж работы с компьютером (в годах): ");
            int teacherComputerExperience = Convert.ToInt32(Console.ReadLine());
            FacultyMembers.Add(new Teacher(teacherId, teacherFullName, teacherBirthDate, teacherGender, teacherComputerExperience));
            break;

        case 4:
            foreach (var educator in FacultyMembers)
            {
                educator.DisplayInfo();
            }
            break;

        case 1:
            Console.WriteLine("Введите идентификатор: ");
            int studentId = Convert.ToInt32(Console.ReadLine());
            if (studentId == 0)
            {
                Console.WriteLine("Идентификатор не может быть пустым");
                break;
            }
            Console.WriteLine("Введите полное имя: ");
            string studentFullName = Console.ReadLine();
            if (string.IsNullOrEmpty(studentFullName))
            {
                Console.WriteLine("Имя не может быть пустым");
                break;
            }
            StudentNamesRegistry.Add(studentFullName);
            Console.WriteLine("Введите дату рождения: (ГГГГ,ММ,ДД)");
            DateOnly studentBirthDate = DateOnly.Parse(Console.ReadLine());
            bool studentDateValid = DateOnly.TryParse(Console.ReadLine(), out studentBirthDate);
            if (!studentDateValid)
            {
                Console.WriteLine("Неверный формат даты");
                break;
            }
            Console.WriteLine("Введите пол: ");
            string studentGender = Console.ReadLine();
            if (string.IsNullOrEmpty(studentGender))
            {
                Console.WriteLine("Пол не может быть пустым");
                break;
            }
            Console.WriteLine("Введите опыт работы с ПК (в годах): ");
            int studentComputerExperience = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите группу здоровья: ");
            string healthCategory = Console.ReadLine();
            if (string.IsNullOrEmpty(healthCategory))
            {
                Console.WriteLine("Группа здоровья не может быть пустой");
                break;
            }
            StudentRoster.Add(new Student(studentId, studentFullName, studentBirthDate, studentGender, studentComputerExperience, healthCategory));
            break;

        case 2:
            foreach (var learner in StudentRoster)
            {
                learner.DisplayInfo();
            }
            break;

        case 5:
            Console.WriteLine("Введите идентификатор: ");
            int courseId = Convert.ToInt32(Console.ReadLine());
            if (courseId == 0)
            {
                Console.WriteLine("Идентификатор не может быть пустым");
                break;
            }
            Console.WriteLine("Введите название курса: ");
            string courseTitle = Console.ReadLine();
            if (string.IsNullOrEmpty(courseTitle))
            {
                Console.WriteLine("Название не может быть пустым");
                break;
            }
            Console.WriteLine("Введите описание курса: ");
            string courseDescription = Console.ReadLine();
            if (string.IsNullOrEmpty(courseDescription))
            {
                Console.WriteLine("Описание не может быть пустым");
                break;
            }
            Console.WriteLine("Введите имя преподавателя курса: ");
            string courseInstructor = Console.ReadLine();
            if (string.IsNullOrEmpty(courseInstructor))
            {
                Console.WriteLine("Имя преподавателя не может быть пустым");
                break;
            }
            Console.WriteLine("Введите имена студентов курса через Enter. Для завершения введите 000: ");
            List<string> enrolledStudents = new List<string>();
            bool addingStudents = true;
            while (addingStudents)
            {
                string inputName = Console.ReadLine();
                if (inputName == "000")
                {
                    CourseCatalog.Add(new Course(courseId, courseTitle, courseDescription, courseInstructor, enrolledStudents));
                    addingStudents = false;
                }
                else if (StudentNamesRegistry.Contains(inputName))
                {
                    enrolledStudents.Add(inputName);
                }
                else
                {
                    Console.WriteLine("Данный студент не зарегистрирован в системе!");
                }
            }
            break;

        case 6:
            foreach (var subject in CourseCatalog)
            {
                subject.DisplayInfo();
            }
            break;

        case 7:
            Console.WriteLine("Введите полное имя для поиска курсов: ");
            string searchName = Console.ReadLine();
            foreach (var subject in CourseCatalog)
            {
                if (subject.EnrolledStudents.Contains(searchName))
                {
                    Console.WriteLine(subject.CourseTitle);
                }
            }
            break;

        case 8:
            foreach (var learner in StudentRoster)
            {
                learner.DisplayInfo();
            }
            foreach (var educator in FacultyMembers)
            {
                educator.DisplayInfo();
            }
            foreach (var subject in CourseCatalog)
            {
                subject.DisplayInfo();
            }
            break;

        case 0:
            programRunning = false;
            break;
    }
}

class Course
{
    private int CourseId;
    public string CourseTitle;
    private string CourseDescription;
    private string InstructorName;
    public List<string> EnrolledStudents;

    public Course(int id, string title, string description, string instructor, List<string> students)
    {
        CourseId = id;
        CourseTitle = title;
        CourseDescription = description;
        InstructorName = instructor;
        EnrolledStudents = students;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Название: {CourseTitle}\nОписание: {CourseDescription}\nПреподаватель: {InstructorName}");
        Console.WriteLine("Зачисленные студенты:");
        foreach (var participant in EnrolledStudents)
        {
            Console.WriteLine(participant);
        }
    }
}

class Person
{
    private string FullName;
    private DateOnly BirthDate;
    private string Gender;

    public Person(string name, DateOnly dateOfBirth, string gender)
    {
        FullName = name;
        BirthDate = dateOfBirth;
        Gender = gender;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"ФИО: {FullName}\nДата рождения: {BirthDate}\nПол: {Gender}");
    }
}

class Teacher : Person
{
    private int TeacherId;
    private int ComputerExperience;

    public Teacher(int id, string name, DateOnly dateOfBirth, string gender, int experience)
        : base(name, dateOfBirth, gender)
    {
        TeacherId = id;
        ComputerExperience = experience;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine("ПРЕПОДАВАТЕЛЬ");
        base.DisplayInfo();
        Console.WriteLine($"ID: {TeacherId}");
        Console.WriteLine($"Опыт работы с компьютером: {ComputerExperience} лет\n");
    }
}

class Student : Person
{
    private int StudentId;
    private int PCExperienceYears;
    private string HealthGroup;

    public Student(int id, string name, DateOnly dateOfBirth, string gender, int computerExperience, string healthGroup)
        : base(name, dateOfBirth, gender)
    {
        StudentId = id;
        PCExperienceYears = computerExperience;
        HealthGroup = healthGroup;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine("СТУДЕНТ");
        base.DisplayInfo();
        Console.WriteLine($"ID студента: {StudentId}");
        Console.WriteLine($"Опыт работы с ПК: {PCExperienceYears} лет");
        Console.WriteLine($"Группа здоровья: {HealthGroup}\n");
    }
}