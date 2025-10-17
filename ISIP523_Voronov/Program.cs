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
        ChooseCharacter();
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

    static void StartGame()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║        ИГРА НАЧИНАЕТСЯ!      ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine($"║ Герой: {characterName,-20}  ║");
        Console.WriteLine($"║ Внешность: {Character}                 ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine("\nЗдесь будет ваше приключение...");
        Console.WriteLine("Нажмите любую клавишу чтобы вернуться в главное меню...");
        Console.ReadKey();
    }

    static void Tutorial()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║          ТУТОРИАЛ            ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine("║ Это игра в жанре RPG.        ║");
        Console.WriteLine("║ Вы выбираете персонажа,      ║");
        Console.WriteLine("║ даёте ему имя и отправляетесь║");
        Console.WriteLine("║ в опасное приключение!       ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ Удачи, путник!               ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();
    }
}