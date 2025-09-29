// See https://aka.ms/new-console-template for more information


using System;
using System.Collections.Generic;
using System.Text;

class TextStat
{
    public string TextPreview { get; set; }
    public int WordCount { get; set; }
    public string ShortWord { get; set; }
    public string LongtWord { get; set; }
    public int SentCount { get; set; }
    public int VowelCount { get; set; }
    public int ConsonantCount { get; set; }
    public Dictionary<char, int> LetterFrequency { get; set; }
    public DateTime AnalysisTime { get; set; }


    public TextStat()
    {
        LetterFrequency = new Dictionary<char, int>();
    }


    public void PrintStat()
    {
        Console.WriteLine($" = Анализ текста = ");
        Console.WriteLine($"Время анализа текста : {AnalysisTime:G}");
        Console.WriteLine($"Просмотр текста: {TextPreview}");
        Console.WriteLine($"Слов в тексте: {WordCount}");
        Console.WriteLine($"Самое короткое слово: {ShortWord}");
        Console.WriteLine($"Самое длинное слово: {LongtWord}");
        Console.WriteLine($"Количество предложений: {SentCount}");
        Console.WriteLine($"Количество гласных букв: {VowelCount}");
        Console.WriteLine($"Количество согласных букв: {ConsonantCount}");

        Console.WriteLine($"Частота букв: ");
        foreach (var entry in LetterFrequency)
        {
            Console.WriteLine($"  {entry.Key}: {entry.Value}");
        }
        Console.WriteLine("=" + new string('=', 100));
    }
}
class TextAnalyzer
{
    private List<TextStat> statisticsHistory = new List<TextStat>();
    private readonly char[] vowels = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я','a', 'e', 'i', 'o', 'u', 'y' };
    private readonly char[] sentenceSeparators = { '.', '!', '?', ';' };
    static void Main(string[] args)
    {
        TextAnalyzer analyzer = new TextAnalyzer();
        analyzer.Run();
    }
    public void Run()
    {
        bool running;
        running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine(" = Анализатор текста = ");
            Console.WriteLine("1. Анализировать новый текст");
            Console.WriteLine("2. Показать историю статистики");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    NewTextAnalyzer();
                    break;
                case "2":
                    ShowStatisticsHistory();
                    break;
                case "3":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Неверный Выбор!");
                    Console.ReadKey();
                    break;
            }
        }





    }


}



