
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

    
}