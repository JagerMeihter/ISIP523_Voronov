using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static Book;

class Book
{
    private Jenre jenre;

    public int BookID { get; set; }
    public string BookName { get; set; }
    public string BookAuthor { get; set; }
    public Jenre BookJenre { get; set; }
    public int BookYear { get; set; }
    public decimal Price { get; set; }
    
    public Book(int bookID , string name , string author , Jenre jenre , int year , decimal price)
    {
        BookID = bookID;
        BookName = name;
        BookAuthor = author;    
        BookJenre = jenre;
        BookYear = year;
        Price = price;
    }

    public Book(int bookID, Jenre jenre)
    {
        BookID = bookID;
        this.jenre = jenre;
    }

    public enum Jenre
    {
        Horror,
        Detective,
        Roman,
        Fantastik,
        Fantasy,
        Religion
        
    }
    public void PrintInfo()
    {
        Console.WriteLine($"ID: {BookID}, Название: {BookName}, Цена: {Price: денег}, Автор: {BookAuthor}, Жанр: {BookJenre}, Год: {BookYear}");
    }
}
class Program
{
    static List<Book> books = new List<Book>();
    static int nextBookId = 1;

    static void Main(string[] args)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n=== Управление Книгами ===");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Удалить книгу");
            Console.WriteLine("3. Поиск книги");
            Console.WriteLine("4. Авторы и книги");
            Console.WriteLine("5. Цены и книги");
            Console.WriteLine("6. Сортировка книг");
            Console.WriteLine("7. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    RemoveBook();
                    break;
                case "3":
                    SearchBook();
                    break;
                case "4":
                    //AuthorBook();
                    break;
                case "5":
                    //PriceBook();
                    break;
                case "6":
                    //SortBook();
                    break;
                case "7":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static int GenerateBookId()
    {
        return nextBookId++;
    }

    static void AddBook()
    {
        try
        {
            int bookID = GenerateBookId();
            Console.WriteLine($"Автоматически сгенерированный ID книги: {bookID}");

            Console.Write("Введите название книги: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название книги не может быть пустым!");
                nextBookId--;
                return;
            }

            Console.Write("Введите автора книги: ");
            string author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("Автор книги не может быть пустым!");
                nextBookId--;
                return;
            }

            Console.Write("Введите год написания книги: ");
            int year = int.Parse(Console.ReadLine());
            if (year < 0)
            {
                Console.WriteLine("Год не может быть отрицательным!");
                nextBookId--;
                return;
            }

            Console.Write("Введите цену книги: ");
            decimal price = decimal.Parse(Console.ReadLine());
            if (price < 0)
            {
                Console.WriteLine("Цена не может быть отрицательной!");
                nextBookId--;
                return;
            }

            Console.Write("Введите жанр книги: \n" +
                "1 - Хоррор\n" +
                "2 - Детектив\n" +
                "3 - Роман\n" +
                "4 - Фантастика\n" +
                "5 - Фентези\n" +
                "6 - Религия\n" +
                "Ваш выбор: ");

            int genreInput = int.Parse(Console.ReadLine());
            if (genreInput < 1 || genreInput > 6)
            {
                Console.WriteLine("Неверный жанр! Выберите от 1 до 6.");
                nextBookId--;
                return;
            }

            Book.Jenre jenre = (Book.Jenre)(genreInput - 1);

            Book newBook = new Book(bookID, name, author, jenre, year, price);
            books.Add(newBook);
            Console.WriteLine("Книга успешно добавлена!");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка ввода! Проверьте правильность данных.");
            nextBookId--;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            nextBookId--;
        }
    }

    static void RemoveBook()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Список книг пуст! Нечего удалять.");
            return;
        }

        // Показываем все товары для удобства выбора
        Console.WriteLine("\nТекущий список книг:");
        foreach (var book in books)
        {
            book.PrintInfo();
        }

        Console.Write("\nВведите ID книги для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int bookId))
        {
            // Ищем товар по ID
            Book bookToRemove = books.Find(p => p.BookID == bookId);

            if (bookToRemove != null)
            {
                // Подтверждение удаления
                Console.Write($"Вы уверены, что хотите удалить книгу '{bookToRemove.BookName}' (ID: {bookToRemove.BookID})? (да/нет): ");
                string confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "да" || confirmation?.ToLower() == "yes")
                {
                    books.Remove(bookToRemove);
                    Console.WriteLine(" Книга успешно удалена!");

                    // Если удаленный товар был с максимальным ID, можно пересчитать nextProductId
                    if (books.Count == 0)
                    {
                        nextBookId = 1;
                    }
                    else
                    {
                        nextBookId = books.Max(p => p.BookID) + 1;
                    }
                }
                else
                {
                    Console.WriteLine(" Удаление отменено.");
                }
            }
            else
            {
                Console.WriteLine("Книга с таким ID не найдена!");
            }
        }
        else
        {
            Console.WriteLine(" Неверный формат ID! Введите целое число.");
        }
    }
    static void SearchBook()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Список книг пуст!");
            return;
        }

        Console.WriteLine("\n=== ПОИСК КНИГ ===");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По автору");
        Console.WriteLine("3. По жанру");
        Console.Write("Выберите тип поиска: ");

        string searchType = Console.ReadLine();
        Console.Write("Введите поисковый запрос: ");
        string searchQuery = Console.ReadLine().ToLower();

        var results = new List<Book>();

        switch (searchType)
        {
            case "1":
                results = books.Where(b => b.BookName.ToLower().Contains(searchQuery)).ToList();
                break;
            case "2":
                results = books.Where(b => b.BookAuthor.ToLower().Contains(searchQuery)).ToList();
                break;
            case "3":
                if (Enum.TryParse<Book.Jenre>(searchQuery, true, out var genre))
                {
                    results = books.Where(b => b.BookJenre == genre).ToList();
                }
                else
                {
                    Console.WriteLine("Жанр не найден!");
                    return;
                }
                break;
            default:
                Console.WriteLine("Неверный тип поиска!");
                return;
        }

        if (results.Count > 0)
        {
            Console.WriteLine($"\n=== НАЙДЕНО КНИГ: {results.Count} ===");
            foreach (var book in results)
            {
                book.PrintInfo();
            }
        }
        else
        {
            Console.WriteLine("Книги не найдены.");
        }
    }
    static void AuthorBook()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Список книг пуст!");
            return;
        }

        Console.WriteLine("\n=== КОЛИЧЕСТВО КНИГ ПО АВТОРАМ ===");
        var authorGroups = books.GroupBy(b => b.BookAuthor)
                               .Select(g => new { Author = g.Key, Count = g.Count() });

        foreach (var group in authorGroups)
        {
            Console.WriteLine($"Автор: {group.Author}, Количество книг: {group.Count}");
        }
    }

    static void PriceBook()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Список книг пуст!");
            return;
        }

        Console.WriteLine("\n=== САМАЯ ДОРОГАЯ И САМАЯ ДЕШЕВАЯ КНИГА ===");

        var mostExpensive = books.OrderByDescending(b => b.Price).First();
        var cheapest = books.OrderBy(b => b.Price).First();

        Console.WriteLine("Самая дорогая книга:");
        mostExpensive.PrintInfo();

        Console.WriteLine("\nСамая дешевая книга:");
        cheapest.PrintInfo();
    }


}




