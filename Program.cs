using System;
using System.Collections.Generic;
using System.Linq;

class ExpenseTracker
{
    class Expense
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    static void Main()
    {
        Console.WriteLine("Добро пожаловать в приложение учета расходов!");
        List<Expense> expenses = new List<Expense>();
        // Ввод количества операций  
        int operationCount;
        do
        {
            Console.Write("Введите количество операций (от 2 до 40): ");
        } while (!int.TryParse(Console.ReadLine(), out operationCount) || operationCount < 2 || operationCount > 40);
        // Ввод операций  
        for (int i = 0; i < operationCount; i++)
        {
            while (true)
            {
                Console.Write($"Введите операцию {i + 1} (Формат: Название; Сумма): ");
                string[] input = Console.ReadLine().Split(';');
                if (input.Length == 2 &&
                    decimal.TryParse(input[1].Trim(), out decimal amount))
                {
                    expenses.Add(new Expense
                    {
                        Name = input[0].Trim(),
                        Amount = amount
                    });
                    break;
                }
                Console.WriteLine("Ошибка формата. Используйте формат: Название; Сумма");
            }
        }
        // Главное меню  
        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт меню: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ShowExpenses(expenses);
                    break;
                case "2":
                    ShowStatistics(expenses);
                    break;
                case "3":
                    BubbleSort(expenses);
                    Console.WriteLine("\nОперации отсортированы по возрастанию цены.");
                    break;
                case "4":
                    ConvertCurrency(expenses);
                    break;
                case "5":
                    SearchByName(expenses);
                    break;
                case "0":
                    Console.WriteLine("Выход из программы.");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
    static void ShowExpenses(List<Expense> expenses)
    {
        Console.WriteLine("\nСписок операций:");
        foreach (var expense in expenses)
        {
            Console.WriteLine($"{expense.Name}: {expense.Amount} руб.");
        }
    }
    static void ShowStatistics(List<Expense> expenses)
    {
        Console.WriteLine("\nСтатистика:");
        Console.WriteLine($"Сумма: {expenses.Sum(e => e.Amount)} руб.");
        Console.WriteLine($"Среднее: {expenses.Average(e => e.Amount)} руб.");
        Console.WriteLine($"Максимальная трата: {expenses.Max(e => e.Amount)} руб.");
        Console.WriteLine($"Минимальная трата: {expenses.Min(e => e.Amount)} руб.");
    }
    static void BubbleSort(List<Expense> expenses)
    {
        for (int i = 0; i < expenses.Count - 1; i++)
        {
            for (int j = 0; j < expenses.Count - i - 1; j++)
            {
                if (expenses[j].Amount > expenses[j + 1].Amount)
                {
                    var temp = expenses[j];
                    expenses[j] = expenses[j + 1];
                    expenses[j + 1] = temp;
                }
            }
        }
    }
    static void ConvertCurrency(List<Expense> expenses)
    {
        Console.WriteLine("\nДоступные валюты:");
        Console.WriteLine("1. Доллары (USD)");
        Console.WriteLine("2. Евро (EUR)");
        Console.WriteLine("3. Фунты (GBP)");
        Console.WriteLine("4. Ввести свой курс");
        Console.Write("Выберите валюту или введите курс (например: 0.012 для долларов): ");
        string currencyChoice = Console.ReadLine();
        try
        {
            decimal rate;
            string currency;

            switch (currencyChoice)
            {
                case "1":
                    rate = 0.012m; // Примерный курс USD  
                    currency = "USD";
                    break;
                case "2":
                    rate = 0.011m; // Примерный курс EUR  
                    currency = "EUR";
                    break;
                case "3":
                    rate = 0.0095m; // Примерный курс GBP  
                    currency = "GBP";
                    break;
                default:
                    if (!decimal.TryParse(currencyChoice, out rate))
                        throw new Exception();
                    currency = "иностранная валюта";
                    break;
            }
            Console.WriteLine($"\nКонвертация по курсу 1 рубль = {rate} {currency}:");
            foreach (var expense in expenses)
            {
                Console.WriteLine($"{expense.Name}: {expense.Amount * rate} {currency}");
            }
        }
        catch
        {
            Console.WriteLine("Ошибка: введите корректное значение");
        }
    }

    static void SearchByName(List<Expense> expenses)
    {
        Console.Write("Введите название для поиска: ");
        string searchTerm = Console.ReadLine().ToLower();

        var foundExpenses = expenses
            .Where(e => e.Name.ToLower().Contains(searchTerm))
            .ToList();

        if (foundExpenses.Any())
        {
            Console.WriteLine("\nНайденные операции:");
            foreach (var expense in foundExpenses)
            {
                Console.WriteLine($"{expense.Name}: {expense.Amount} руб.");
            }
        }
        else
        {
            Console.WriteLine("Совпадений не найдено.");
        }
    }
}