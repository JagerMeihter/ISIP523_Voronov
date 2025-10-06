using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static Book;
class Book
{
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
    public enum Jenre
    {
        Horror,
        Detective,
        Roman,
        Fantastik,
        Fantasy
        
    }
    public void PrintInfo()
    {
        Console.WriteLine($"ID: {BookID}, Название: {BookName}, Цена: {Price: денег}, Автор: {BookAuthor}, Жанр: {BookJenre}, Год: {BookYear}");
    }
}
class Program
{
    static List<Book> products = new List<Book>();
    static int nextProductId = 1; // Счетчик для автоматической генерации ID
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
                    AuthorBook();
                    break;
                case "5":
                    PriceBook();
                    break;
                case "6":
                    SortBook();
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
    ы
    









}

