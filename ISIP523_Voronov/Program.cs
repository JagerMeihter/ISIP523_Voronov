using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Quic;
using System.Net.WebSockets;
using System.Text;

public static class Function
{
    public static int RollDice20()
    {
        Random random = new Random();
        return random.Next(1, 21);
        
    }
}

class Program
{
    static string characterName = "";
    static int Character;
    static int QuantityHP;
    static List<string> inventory = new List<string>();
    // Массив с внешностями персонажей
    static readonly string[] characterAppearances = {
        // 0 - пустой элемент (индексы с 1)
        "",
        
        // 1
        @"⢡⣷⣾⡷⣿⡿⢟⡿⠿⡿⠟⠛⠛⠛⠻⠿⣿⣿⣿⣿⣤⣶⣾⠶⣿⡄⠘⢿
⠀⠀⠀⠀⠉⠀⠈⠀⠋⢀⣶⣶⣥⣀⣄⠀⠀⠙⣿⣿⣿⣿⣿⣷⣿⣷⠀⠘
⠀⠀⠀⠀⠀⠀⠀⢠⠤⡄⠙⠉⢾⣏⣙⠂⠀⠀⠘⢿⣿⡟⠻⠆⠉⠛⠃⠀
⠀⠀⠀⠀⠀⠀⠐⣧⣤⡄⠀⠀⠀⣦⠙⠻⡿⠾⣆⡀⠻⠇⠀⠀⠀⠀⣰⠀
⠀⠀⠀⠀⠀⠀⠰⠟⣡⣤⣴⣾⣿⣿⣷⣄⠐⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⡀⠀⠀⠀⠀⠀⠀⠘⣿⣿⣿⣿⣿⣿⣿⣿⣆⠠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠻⢷⡄⠀⠀⠀⠀⠸⣿⣿⣿⣿⣿⠿⠋⠉⣉⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣿⣆⡀⠀⠀⠀⠀⠀⠀⠀⠈⣿⣦⣤⣾⣫⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣛⣿⠁⠀⠀⠀⠀⠠⣤⣾⡏⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣿⣿⣧⡄⠀⠀⠀⠀⠘⣿⡗⠻⠿⢛⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣠⠉⠀⠀⠀⠀⠀⠀⠀⠸⠗⠀⠀⠈⢙⣋⠻⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣧⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠂⠀⠙⠋⣛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠻⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠿⠟⠛⠛⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢢⠀⠀⠀⡀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠘⣀⠀⠀⠀⠀⠀⣀⠀⠀⠀⠀⠀⠈⠀⠀⠀⢾⣏⠉
⠀⠀⠀⠁⠀⠀⠀⠐⣤⡀⣥⡀⠀⢳⣶⣿⢇⡒⠀⡤⠀⠀⠀⠐⠒⠤⠈⠃",
        
        // 2
        @"⣿⣿⣿⣿⣿⣿⣿⣿⠿⢋⣽⣿⣿⣿⣿⣿⣿⣿⣿⠿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⠋⡀⠀⠛⠛⢿⣿⣿⣿⡿⠿⢛⣩⡶⢍⢻⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⡿⠋⠀⣀⠙⢲⣦⣴⣌⡹⣷⡾⢻⣽⣿⣷⣾⣿⣿⣿⣿⣿⣿⡦
⣿⡿⡿⣿⡇⠀⠀⠙⣿⣿⣿⠽⢿⣻⣷⣟⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡅
⡿⠁⡁⠻⠁⠀⠀⠀⠹⣿⣿⡆⢻⢿⣦⣿⣇⢹⣿⣿⣿⣿⣿⡏⠁⠙⠛⠁
⠃⠀⠀⠀⠀⠀⠀⠀⣀⣬⡉⠁⠘⠻⣾⣏⠀⠻⠿⣿⣿⣧⣿⣿⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠈⠉⠁⠀⠀⠀⠈⢀⣀⡀⠀⠀⠀⠉⢻⣬⡟⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠸⣦⡄⠀⠀⠀⠀⠘⢿⣇⣀⡀⠀⠀⢸⣿⠄⠀⠀⠀⠀
⡀⢀⡀⢠⣤⣶⠀⠀⠛⠲⠒⠲⠇⠀⠀⠀⣿⣿⠗⠒⠲⣿⣿⣧⡀⢀⠀⠀
⠛⠻⠿⠀⠀⠿⠀⠀⢠⣶⣦⠀⠀⢀⠀⡀⣸⣿⣤⣀⠙⢿⡿⢧⣽⣿⠆⠀
⠀⠀⠀⠀⠀⠀⠀⣷⠀⠀⢨⠀⢠⣷⣤⣿⣿⣿⣿⢹⢠⣿⠀⠀⠛⠃⠀⠀
⠀⠀⠀⢀⠀⠀⠀⢀⡀⠀⠀⠄⢨⣽⣭⣹⣵⢶⣾⣀⣾⣧⣴⡿⣿⠀⣼⣿
⠀⠀⠀⣿⣧⡄⢀⣍⠁⠀⠀⠀⠀⢠⣼⣬⣖⡛⣹⣿⣿⣤⣾⣿⣿⠐⣿⣿
⠀⠀⠘⠛⠋⢠⠞⠁⠀⠀⠀⠀⠀⢸⣷⣶⣾⣿⣿⡿⠋⠉⠉⠻⣏⠀⠟⠿
⠀⠀⠀⢠⡀⠀⠁⠂⠀⠀⠀⠀⠀⠹⠿⠿⠿⠛⠉⠀⠀⠀⠀⠀⢨⠃⠀⠀
⠀⠀⠔⠊⠑⠦⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠤⠀⠀⠀⠀⠁⠀⠈⠳",
        
        // 3
        @"⣿⣦⣯⣿⣿⣝⣾⣿⣿⣿⡏⠉⠈⠁⠈⠉⠉⠈⠙⣿⣿⣿⡱⠂⠀⠀⠀⠀
⣿⡿⠿⠛⠉⠉⠁⠙⢉⣽⠾⢷⡆⣠⣶⣾⣿⣷⣦⣌⠙⠻⣿⣦⠀⣀⠀⠀
⣿⣿⣿⣶⠀⠀⢀⣼⣿⣿⠄⠀⠀⣹⡿⠿⣿⣿⣿⣿⠗⠀⢹⣿⣿⣷⣶⣦
⣿⣿⣿⣿⠀⣰⣿⣿⠟⠛⠷⠀⠐⣉⣀⣀⠀⠙⠆⠉⠶⠀⠘⢿⣻⣿⣏⠡
⣿⣿⣿⣿⣄⣿⣿⡅⢠⣇⣴⣶⣾⣿⣿⣿⡿⠀⠀⠀⠀⠀⠀⠊⠀⠈⠁⢰
⣍⢻⣿⣿⡏⠙⣿⡅⢺⣿⣿⣿⣿⣿⢻⣿⣯⣀⡀⠀⠀⠀⠀⠀⠀⠀⢠⣤
⠿⣆⣿⣿⣿⣄⣿⣿⠈⣻⣿⣿⣟⣛⣽⣿⠟⠋⠁⠀⠀⠀⠀⠳⢀⡀⢸⣿
⡄⠡⡜⢿⣿⣿⣿⡟⠺⡿⠿⠟⠻⢿⣏⠀⢠⣄⢆⣰⠆⠀⠀⡠⠀⣿⠈⠀
⣿⣆⠂⠀⠹⣿⣿⣧⠀⣀⣰⣴⣶⣸⣿⣿⢻⣿⣿⣿⠀⠀⠈⠁⠀⠉⠀⠀
⣾⣿⣅⠀⠀⠙⢿⣿⣿⢻⣿⣿⣿⣿⣿⣿⡿⣿⣿⣿⡂⠀⠀⠀⠀⠀⠀⠀
⢻⢿⡟⢧⣀⣰⡿⣿⣿⡦⢿⣿⣿⣯⣍⣉⣠⣾⢿⣿⡇⠀⠀⠀⠀⠀⠀⢀
⣿⣸⣟⣿⡽⢿⣷⣻⣿⣷⡈⢿⣿⡿⢛⣩⣥⢴⣾⣿⡇⠀⠀⠀⢀⠀⡀⠀
⡫⡈⠹⢏⡅⢪⣿⣯⠿⠿⡃⠀⠙⠿⣿⣿⣶⣿⣿⣿⠃⠀⠀⠀⡾⢰⡛⠠
⢰⠊⠁⠸⡔⠻⠓⠀⣠⣶⣨⣤⠖⢀⡙⠻⡿⠛⠉⠀⠀⠀⢀⣼⢷⣸⣿⠐
⡂⠀⠀⠀⠠⠂⢠⣴⣿⣿⣿⣝⡃⢠⣤⣹⣿⣿⣶⣤⣀⡂⡟⢻⣿⠟⠙⠈
⠈⠁⠀⠀⠇⠠⣶⣜⡛⠻⢿⠏⣁⠀⠴⢿⣿⣿⣿⣻⣝⣤⡾⠚⠀⠀⠀⠀",
        
        // 4
        @"⣿⣿⣿⣿⣿⣿⠟⠛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⡟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⣿⣿⣿⣿
⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⣀⡀⠀⠀⠀⠈⣿⣿⣿⣿⣿
⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠡⠀⠀⠀⠘⣿⣿⣿⣿
⣿⣿⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣿⢿⠛
⣿⣿⠇⠀⠀⠀⡀⠀⠀⠀⠀⠀⠀⣤⣄⠀⠀⠀⠀⠀⢘⠀⠀⠀⠐⠀⠀⠀
⣿⡿⠀⠀⠀⠀⡇⠀⢀⣴⣶⣿⡏⢻⣿⣧⣄⡀⠀⠀⠀⠄⠀⠀⠀⠀⠀⠀
⠉⠁⠀⠀⠀⠀⠁⠀⠘⣿⣿⣿⠁⠘⢛⣿⣿⣷⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠐⠀⠀⠀⣿⣧⣴⣿⣿⣿⣿⣿⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣽⠇⣡⣴⣦⣭⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣷⣶⣶⣶⣾⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠻⢿⣿⣿⠿⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠰⡀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣴⣷⠀⠀⠀⠀⠀⠀⠀⢃⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣴⣿⣿⣿⣿⣦⠀⠀⠀⠀⠀⠀⠐⡂⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢶⣿⣿⣿⣿⡟⠁⠀⠀⠀⠀⠀⠀⠀⣘⡄⠀",
        
        // 5
        @"⠀⠰⢠⠄⠀⠀⠀⠀⢸⣿⣿⣛⣋⡉⠉⠉⠉⠙⠛⠛⠛⢻⣿⣿⣿⣾⠇⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠘⠓⠉⢛⣿⣿⠋⠀⣀⣠⡀⠀⠀⠸⠿⣿⣿⡿⠤⠃
⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠸⠟⢁⣤⣾⣿⣿⣧⣼⣶⣦⠀⠀⣿⣿⣖⣖
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⢠⠄⣿⣿⣿⡈⢿⣿⣿⣿⣧⠀⠹⢿⣿⡟
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠴⣏⡀⢰⣦⡙⣿⣿⣿⡖⣿⣿⣿⠟⣃⡈⢀⠋⡞
⠀⠀⠀⡀⠀⣀⠀⠀⠀⠀⠘⠀⠸⣿⣃⠛⠻⣿⣼⣿⣿⠁⢈⡉⠋⠉⡧⠤
⡴⠿⠿⠿⠀⠈⡦⠀⠀⠀⠀⠀⣀⠉⠉⢀⣤⣙⣿⣿⣷⣶⣾⣷⡇⠀⢶⣴
⠀⠀⠀⠀⠀⠀⢷⣀⢀⣀⠀⠂⢻⣾⣿⣿⣿⣿⣇⣻⣿⣿⣿⣿⡇⠀⠀⣦
⠀⠀⠀⠀⠀⠀⠀⠀⠈⠁⠀⠀⠈⠛⢿⣿⣿⣿⣿⣬⣉⣯⣽⣿⡇⠀⠠⠟
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠢⠠⣀⣬⣿⣿⣿⣿⡿⠛⢛⠙⠻⡇⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⢿⣿⣿⣿⣯⣤⣼⣿⣷⣾⠇⠀⠶⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠻⠿⡿⠿⣿⣿⣿⠏⠀⣠⠠⣞
⢀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣁⡘⢻
⠀⠉⠉⠒⠦⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⡄⠀⠀⠀⠀⠀⠀⠐⠦⠵⠏
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠒⠂⠀⠒⠂⠀⠀⠀"
    };

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        bool running = true;

        while (running)
        {
            ShowMainMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GameStart();
                    break;
                case "2":
                    Tutorial();
                    break;
                case "3":
                    running = false;
                    Console.WriteLine("Боишься?! картинки тут из точечек ты серьёзно?");
                    break;
                default:
                    Console.WriteLine("Такого не дано, выбирай давай!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ShowMainMenu()
    {
        Console.Clear();
        int result = Function.RollDice20();
        Console.WriteLine("  ┌─┐┌─┐┬─┐┌─┐┌─┐┬");
        Console.WriteLine("  │ ┬├┤ ├┬┘│  ├─┤│");
        Console.WriteLine("  └─┘└─┘┴└─└─┘┴ ┴┴");
        Console.WriteLine("  ┬  ┌─┐┌─┐┌─┐┬");
        Console.WriteLine("  │  │ ││  ├─ │");
        Console.WriteLine("  ┴─┘└─┘└─┘┴  ┴");
        Console.WriteLine("Добро пожаловать! Хотите испытать себя в битве? За последний глоток ВОЛИ?");
        Console.WriteLine("1. Я готов! (начать новую игру)");
        Console.WriteLine("2. Что? (Туториал)");
        Console.WriteLine("3. Не, я струсил (Выйти)");
        Console.Write("Ваш выбор: ");
    }

    static void GameStart()
    {
        ChooseCharacter(); //создание персонажа
        InitializeStarterInventory(); // инвентарь
        CharacterManagementMenu(); // управление персонажем

    }

    static void ChooseCharacter()
    {
        bool choosing = true;

        while (choosing)
        {
            Console.Clear();
            Console.WriteLine("Выберите своего персонажа:");

            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"{i}.");
                Console.WriteLine(characterAppearances[i]);
                Console.WriteLine();
            }
            Console.WriteLine("6. Назад в главное меню");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                    Character = int.Parse(choice);
                    NameCharacter();
                    choosing = false;
                    break;
                case "6":
                    choosing = false;
                    break;
                default:
                    Console.WriteLine("Такого не дано, выбирай давай!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void NameCharacter()
    {
        bool namingComplete = false;
        QuantityHP = 20;
        while (!namingComplete)
        {
            Console.Clear();
            Console.WriteLine("Как тебя зовут путник?");
            string input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                characterName = input;
                namingComplete = true;
                ShowSuccessMessage();
                ShowCharacter();
            }
            else
            {
                Console.WriteLine("Имя не может быть пустым!");
                Console.ReadKey();
            }
        }
    }

    static void ShowSuccessMessage()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✓ Имя \"{characterName}\" успешно сохранено!");
        Console.ResetColor();
        System.Threading.Thread.Sleep(1500);
    }

    static void ShowCharacter()
    {
        Console.Clear();
        Console.WriteLine("Это ты?");
        Console.WriteLine($"Имя: {characterName}");
        Console.WriteLine($"HP: {QuantityHP}");
        Console.WriteLine($"Внешность:\n");

        if (Character >= 1 && Character <= 5)
        {
            Console.WriteLine(characterAppearances[Character]);
        }

        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();

        // После показа персонажа можно перейти к самой игре
        StartGame();
    }
    static void InitializeStarterInventory()
    {
        // Очищаем инвентарь перед добавлением стартовых предметов
        inventory.Clear();

        // Добавляем стартовые предметы
        inventory.Add("🗡️Меч    | Мой старый Герой |");
        inventory.Add("🛡️Доспех | Мой старый Герой |");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n🎁 Вам выданы стартовые предметы!");
        Console.ResetColor();
        System.Threading.Thread.Sleep(1500);
    }
    static void StartGame()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║        ИГРА НАЧИНАЕТСЯ!      ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine($"║ Герой: {characterName,-20}  ║");
        Console.WriteLine($"║ Внешность: {Character}          ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine("\nЗдесь будет ваше приключение...");
        Console.WriteLine("Нажмите любую клавишу чтобы вернуться в главное меню...");
        Console.ReadKey();
    }
    static void CharacterManagementMenu()
    {
        bool managing = true;

        while (managing)
        {
            Console.Clear();
            ShowCharacterStatus();
            Console.WriteLine("\n╔══════════════════════════════╗");
            Console.WriteLine("║     УПРАВЛЕНИЕ ПЕРСОНАЖЕМ    ║");
            Console.WriteLine("╠══════════════════════════════╣");
            Console.WriteLine("║ 1. Посмотреть инвентарь      ║");
            Console.WriteLine("║ 2. Начать приключение        ║");
            Console.WriteLine("║ 3. В главное меню            ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowInventory();
                    break;
                case "2":
                    StartAdventure();
                    managing = false;
                    break;
                case "3":
                    managing = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║          ИНВЕНТАРЬ           ║");
        Console.WriteLine("╠══════════════════════════════╣");

        if (inventory.Count == 0)
        {
            Console.WriteLine("║       Инвентарь пуст        ║");
        }
        else
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine($"║ {i + 1,2}. {inventory[i],-22} ║");
            }
        }

        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine($"║ Всего предметов: {inventory.Count,-11} ║");
        Console.WriteLine("╚══════════════════════════════╝");

        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();
    }
    static void ShowCharacterStatus()
    {
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║        ВАШ ПЕРСОНАЖ          ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine($"║ Имя: {characterName,-23} ║");
        Console.WriteLine($"║ Внешность: {Character,-18} ║");
        Console.WriteLine($"║ Предметов: {inventory.Count,-18} ║");
        Console.WriteLine("╚══════════════════════════════╝");

        // Показываем внешность персонажа
        if (Character >= 1 && Character <= 5)
        {
            Console.WriteLine("\nВаш облик:");
            Console.WriteLine(characterAppearances[Character]);
        }
    }

    static void StartAdventure()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║        НАЧАЛО ИГРЫ!          ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine($"║ Герой: {characterName,-23} ║");
        Console.WriteLine($"║ Предметов: {inventory.Count,-18} ║");
        Console.WriteLine("╚══════════════════════════════╝");

        Console.WriteLine("\nВы стоите у входа в тёмный лес...");
        Console.WriteLine("Ваше приключение начинается!");
        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();

        // Здесь будет твоя основная игровая логика
        //FirstEncounter();
    }
    static void Tutorial()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║           ТУТОРИАЛ           ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine("║ Это игра в жанре Roguelike.  ║");
        Console.WriteLine("║ Вы выбираете персонажа,      ║");
        Console.WriteLine("║ даёте ему имя и отправляетесь║");
        Console.WriteLine("║ в опасное приключение!       ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ Вас ждут страшные чудища и   ║");
        Console.WriteLine("║ другие уродские создания.    ║");
        Console.WriteLine("║ Станьте светочом во тьме, и  ║");
        Console.WriteLine("║ навсегда изгоните скверну с  ║");
        Console.WriteLine("║ королевских земель.          ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ Если вы играли в D&D то вам  ║");
        Console.WriteLine("║ будет нетрудно понять систему║");
        Console.WriteLine("║ дайса на d20. Однако если вы ║");
        Console.WriteLine("║ не знакомы с системой дайса  ║");
        Console.WriteLine("║ то тут будет расписан гайд   ║");
        Console.WriteLine("║ чисто ради вашего интереса   ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ Система d20 (20-гранный дайс)║");
        Console.WriteLine("║ это основа многих настольных ║");
        Console.WriteLine("║ RPG особенно системы Dungeons║");
        Console.WriteLine("║ & Dragons (D&D).Вот как она  ║");
        Console.WriteLine("║ работает:                    ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ d20 = 20-гранный игральный   ║");
        Console.WriteLine("║ кубик.                       ║");
        Console.WriteLine("║ Диапазон значений: от 1 до 20║");
        Console.WriteLine("║ Критический успех: 20        ║");
        Console.WriteLine("║ (натуральная 20)             ║");
        Console.WriteLine("║ Критический провал: 1        ║");
        Console.WriteLine("║ (натуральная 1)              ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ ПРИМЕР:                      ║");
        Console.WriteLine("║ Результат= d20 + модификаторы║");
        Console.WriteLine("║ стемасложности Сложность (DC)║");
        Console.WriteLine("║ (DC - Difficulty Class)      ║");
        Console.WriteLine("║ 5-10 Очень легко             ║");
        Console.WriteLine("║ 10-15      Легко             ║");
        Console.WriteLine("║ 15-20      Средне            ║");
        Console.WriteLine("║ 20-25      Сложно            ║");
        Console.WriteLine("║ 25-30      Очень сложно      ║");
        Console.WriteLine("║ 30+  Практически невозможно  ║");
        Console.WriteLine("║ Успех: Результат броска ≥ DC ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ Модификаторы бывают разные,  ║");
        Console.WriteLine("║ ПОЛОЖИТЕЛЬНЫЕ И ОТРИЦАТЕЛЬНЫЕ║");
        Console.WriteLine("║ они прибавляются или         ║");
        Console.WriteLine("║ отнимаются от вашего значения║");
        Console.WriteLine("║ d20 которое вам выпало       ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ Удачи, путник!               ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();
    }
}