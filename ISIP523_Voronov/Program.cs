using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Quic;
using System.Net.WebSockets;

public static class Function
{
    public static int RollDice20()
    {
        Random random = new Random();
        return random.Next(1, 21);
    }

    public static  int GameStart()
    {

        running = true;

    }
    public static int Tutorial()
    {

    }
    public static int Exit()
    {
        running = false;
    }
}


class Program 
{
    static void Main()
    {
        Console.WriteLine("  ┌─┐┌─┐┬─┐┌─┐┌─┐┬\r\n  │ ┬├┤ ├┬┘│  ├─┤│\r\n  └─┘└─┘┴└─└─┘┴ ┴┴\r\n  ┬  ┌─┐┌─┐┌─┐┬\r\n  │  │ ││  ├─ �│\r\n  ┴─┘└─┘└─┘┴  ┴");
        Console.WriteLine("Добро пожаловать! Хотите испытать се5бя в битве? За пследний глоток ВОЛИ? (выберите вариант для продолжения)");
        Console.WriteLine("1. Я готов! (начать новую игру)");
        Console.WriteLine("2. Что? (Туториал)");
        Console.WriteLine("3. Не, я струсил (Выйти)");






        int result = Function.RollDice20();
        //Console.WriteLine($"Результат броска: {result}");
    }

}


