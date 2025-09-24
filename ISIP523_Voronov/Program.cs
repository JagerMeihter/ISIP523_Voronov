using System;
using System.Collections.Generic;
using System.Linq;
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
    public Product(int productId, string name, decimal price, int quantity, ProductCategory Category, bool haveStorage)
    {
        ProductID = productId;
        Name = name;
        Price = price;
        Quantity = quantity;
        HaveStorage = haveStorage;
        ProductType = Category;
    }

    public enum ProductCategory
    {
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
        Console.WriteLine($"ID: {ProductID}, Название: {Name}, Цена: {Price: денег}, Количество: {Quantity}, Категория товара: {ProductType}, Наличие на складе: {HaveStorage}");
    }
}

class Program
{
    static List<Product> products = new List<Product>();
    static int nextProductId = 1; // Счетчик для автоматической генерации ID
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

    // Метод для генерации уникального ID
    static int GenerateProductId()
    {
        return nextProductId++;
    }

    // Добавить новый товар
    static void AddProduct()
    {
        try
        {
            // Автоматическая генерация ID
            int productId = GenerateProductId();
            Console.WriteLine($"Автоматически сгенерированный ID товара: {productId}");

            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();

            // Проверка на пустое название
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine(" Название товара не может быть пустым!");
                nextProductId--; // Откатываем счетчик, так как товар не был добавлен
                return;
            }

            Console.Write("Введите цену товара: ");
            decimal price = decimal.Parse(Console.ReadLine());
            if (price < 0)
            {
                Console.WriteLine(" Цена не может быть отрицательной!");
                nextProductId--; // Откатываем счетчик
                return;
            }

            Console.Write("Введите количество: ");
            int quantity = int.Parse(Console.ReadLine());
            if (quantity < 0)
            {
                Console.WriteLine(" Количество не может быть отрицательным!");
                nextProductId--; // Откатываем счетчик
                return;
            }

            bool haveStorage = quantity > 0;

            Console.Write("Введите категорию товара: \n" +
                "1 - Выпечка\n" +
                "2 - Молочные продукты\n" +
                "3 - Рыба и Мясо\n" +
                "4 - Сладости\n" +
                "5 - Фрукты\n" +
                "6 - Овощи\n" +
                "Ваш выбор: ");

            int categoryInput = int.Parse(Console.ReadLine());
            if (categoryInput < 1 || categoryInput > 6)
            {
                Console.WriteLine(" Неверная категория! Выберите от 1 до 6.");
                nextProductId--; // Откатываем счетчик
                return;
            }

            ProductCategory category = (ProductCategory)(categoryInput - 1);

            Product newProduct = new Product(productId, name, price, quantity, category, haveStorage);
            products.Add(newProduct);
            Console.WriteLine(" Товар успешно добавлен!");
        }
        catch (FormatException)
        {
            Console.WriteLine(" Ошибка ввода! Проверьте правильность данных.");
            nextProductId--; // Откатываем счетчик при ошибке
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            nextProductId--; // Откатываем счетчик при ошибке
        }
    }

    // Удалить товар (по ID)
    static void RemoveProduct()
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Список товаров пуст! Нечего удалять.");
            return;
        }

        // Показываем все товары для удобства выбора
        Console.WriteLine("\nТекущий список товаров:");
        foreach (var product in products)
        {
            product.PrintInfo();
        }

        Console.Write("\nВведите ID товара для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            // Ищем товар по ID
            Product productToRemove = products.Find(p => p.ProductID == productId);

            if (productToRemove != null)
            {
                // Подтверждение удаления
                Console.Write($"Вы уверены, что хотите удалить товар '{productToRemove.Name}' (ID: {productToRemove.ProductID})? (да/нет): ");
                string confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "да" || confirmation?.ToLower() == "yes")
                {
                    products.Remove(productToRemove);
                    Console.WriteLine(" Товар успешно удален!");

                    // Если удаленный товар был с максимальным ID, можно пересчитать nextProductId
                    if (products.Count == 0)
                    {
                        nextProductId = 1;
                    }
                    else
                    {
                        nextProductId = products.Max(p => p.ProductID) + 1;
                    }
                }
                else
                {
                    Console.WriteLine(" Удаление отменено.");
                }
            }
            else
            {
                Console.WriteLine("Товар с таким ID не найден!");
            }
        }
        else
        {
            Console.WriteLine(" Неверный формат ID! Введите целое число.");
        }
    }

    static void SearchProducts()
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Список товаров пуст!");
            return;
        }

        Console.Write("Введите название товара для поиска: ");
        string searchName = Console.ReadLine();

        bool found = false;

        Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ПОИСКА ===");
        foreach (var product in products)
        {
            if (product.Name.ToLower().Contains(searchName.ToLower()))
            {
                product.PrintInfo();
                Console.WriteLine("-------------------");
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Товары с таким названием не найдены.");
        }
    }

    static void DeliveryProducts()
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Список товаров пуст! Нечего доставлять.");
            return;
        }

        try
        {
            Console.WriteLine("\nТекущий список товаров:");
            foreach (var product in products)
            {
                product.PrintInfo();
            }

            Console.Write("\nВведите ID товара для доставки: ");
            int productId = int.Parse(Console.ReadLine());

            Product productToDeliver = products.Find(p => p.ProductID == productId);

            if (productToDeliver != null)
            {
                Console.Write("Введите адрес доставки: ");
                string address = Console.ReadLine();

                // Проверка на пустой адрес
                if (string.IsNullOrWhiteSpace(address))
                {
                    Console.WriteLine("Адрес не может быть пустым!");
                    return;
                }

                Console.Write("Введите время доставки: ");
                string deliveryTime = Console.ReadLine();

                // Проверка на пустое время
                if (string.IsNullOrWhiteSpace(deliveryTime))
                {
                    Console.WriteLine("Время доставки не может быть пустым!");
                    return;
                }

                // Подтверждение доставки
                Console.WriteLine($"\n=== ПОДТВЕРЖДЕНИЕ ДОСТАВКИ ===");
                Console.WriteLine($"Товар: {productToDeliver.Name}");
                Console.WriteLine($"Количество: {productToDeliver.Quantity} шт.");
                Console.WriteLine($"Адрес: {address}");
                Console.WriteLine($"Время: {deliveryTime}");
                Console.Write($"\nПодтвердить доставку? (да/нет): ");

                string confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "да" || confirmation?.ToLower() == "yes")
                {
                    Console.WriteLine($"\n Доставка товара '{productToDeliver.Name}' назначена!");
                    Console.WriteLine($" Адрес: {address}");
                    Console.WriteLine($" Время: {deliveryTime}");
                    Console.WriteLine($" Количество: {productToDeliver.Quantity} шт.");

                    // Обновляем статус склада
                    productToDeliver.HaveStorage = true;
                    Console.WriteLine(" Статус склада обновлен: товар доступен на складе");
                }
                else
                {
                    Console.WriteLine(" Доставка отменена.");
                }
            }
            else
            {
                Console.WriteLine("Товар с таким ID не найден!");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка ввода! Проверьте правильность данных.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static void SellProducts()
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Нечего продавать!");
            return;
        }

        try
        {
            Console.WriteLine("\n Текущий список товаров: ");
            foreach (var product in products)
            {
                product.PrintInfo();
            }

            Console.Write("\nВведите ID товара для продажи: ");
            int productId = int.Parse(Console.ReadLine());

            Product productToSell = products.Find(p => p.ProductID == productId);

            if (productToSell != null)
            {
                Console.Write($"Введите количество '{productToSell.Name}' которое вы хотите продать: ");
                int productSellCount = int.Parse(Console.ReadLine());

                if (productSellCount <= 0)
                {
                    Console.WriteLine("Количество должно быть положительным числом!");
                    return;
                }

                if (productSellCount > productToSell.Quantity)
                {
                    Console.WriteLine($"Недостаточно товара! В наличии: {productToSell.Quantity} шт.");
                    return;
                }

                decimal totalPrice = productSellCount * productToSell.Price;

                // Подтверждение продажи
                Console.WriteLine($"\n=== ПОДТВЕРЖДЕНИЕ ПРОДАЖИ ===");
                Console.WriteLine($"Товар: {productToSell.Name}");
                Console.WriteLine($"Цена за единицу: {productToSell.Price:C}");
                Console.WriteLine($"Количество: {productSellCount} шт.");
                Console.WriteLine($"Общая сумма: {totalPrice: денег}");
                Console.Write($"\nПодтвердить продажу? (да/нет): ");

                string confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "да" || confirmation?.ToLower() == "yes")
                {
                    productToSell.Quantity -= productSellCount;
                    productToSell.HaveStorage = productToSell.Quantity > 0;

                    Console.WriteLine($" Продажа завершена!");
                    Console.WriteLine($"Остаток товара: {productToSell.Quantity} шт.");
                    Console.WriteLine($"Выручка: {totalPrice: денег}");
                }
                else
                {
                    Console.WriteLine(" Продажа отменена.");
                }
            }
            else
            {
                Console.WriteLine("Товар с таким ID не найден!");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка ввода! Проверьте правильность данных.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}


