using System;
using System.Collections.Generic;
using System.ComponentModel;
using static Product;

class Product
{
    // Свойства класса
    public int ProductID { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool HaveStorage { get; set; }

    public ProductCategory ProductType { get; set; }
    // Конструктор для инициализации всех свойств
    public Product(int productId, string name, decimal price, int quantity, ProductCategory Category , bool haveStorage)

    {
        ProductID = productId;
        Name = name;
        Price = price;
        Quantity = quantity;
        HaveStorage = haveStorage;
        ProductType = Category;

    }
    public enum ProductCategory{
        Bäckerei,
        Milchprodukte,
        Fisch_und_Fleisch,
        Süßigkeiten,
        Früchte,
        Gemüse
    }
    // Метод для вывода информации о товаре
    public void PrintInfo()
    {
        Console.WriteLine($"ID: {ProductID}, Название: {Name}, Цена: {Price:C}, Количество: {Quantity} ,Категория товара:{ProductType}, Наличие на складе:{HaveStorage} ");
    }
}

class Program
{
    static List<Product> products = new List<Product>();

    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n=== Управление товарами ===");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать доставку товара");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товаров");
            Console.WriteLine("6. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddProduct();
                    break;
                case "2":
                    RemoveProduct();
                    break;
                case "3":
                    DeliveryProducts();
                    break;
                case "4":
                    SellProducts();
                    break;
                case "5":
                    SearchProducts();
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }
    
    // Метод для добавления нового товара
    static void AddProduct()
    {
        try
        {
            Console.Write("Введите ID товара: ");
            int productId = int.Parse(Console.ReadLine());

            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();

            Console.Write("Введите цену товара: ");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Введите количество: ");
            int quantity = int.Parse(Console.ReadLine());
            bool haveStorage = true;
            if (quantity <= 0) {
                 haveStorage = false;
            }

            
            Console.Write("Введите категорию товара: " +
                "1 -  Выпечка\n" +
                "2 -  Молочные продукты\n" +
                "3 -  Рыба и Мясо\n" +
                "4 -  Сладости\n" +
                "5 -  Фрукты\n" +
                "6 -  Овощи \n"
                );
            ProductCategory category = (ProductCategory)int.Parse(Console.ReadLine());


            Product NewProduct = new Product(productId,name,price,quantity, category,haveStorage);
            Console.WriteLine("Товар успешно добавлен!");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка ввода! Проверьте правильность данных.");
        }
        
    }

    static void RemoveProduct()
    {
        
    }

    static void SearchProducts()
    {
        
    }
    static void DeliveryProducts()
    {

    }
    static void SellProducts()
    {

    }
}



