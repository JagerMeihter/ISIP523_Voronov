// See https://aka.ms/new-console-template for more information


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

class TextStat
{
    public string TextPreview { get; set; }
    public int WordCount { get; set; }
    public string ShortWord { get; set; }
    public string LongWord { get; set; }
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
        Console.WriteLine($"Самое длинное слово: {LongWord}");
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
    private readonly char[] vowels = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я', 'a', 'e', 'i', 'o', 'u', 'y' };
    private readonly char[] sentenceSeparators = { '.', '!', '?', ';', ':',',' };
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
                    //ShowStatisticsHistory();
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
    private void NewTextAnalyzer()
    {
        Console.Clear();
        Console.WriteLine("Введите новый текст (минимум 100 символов): ");

        string text;
        do
        {
            text = Console.ReadLine();
            if (text == null || text.Length < 100)
            {
                Console.WriteLine($"Текст слишком короткий! Введено {text?.Length ?? 0} символов. Нужно минимум 100.");
                Console.WriteLine("Пожалуйста, введите текст еще раз:");
            }
        } while (text == null || text.Length < 100);

        TextStat analyze = AnalyzeText(text);
        statisticsHistory.Add(stats);

        Console.WriteLine("\nАнализ завершен!");
        stats.PrintStatistics();

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
    private TextStat AnalyzeText(string text)
    {
        TextStat stats = new TextStat();
        stats.AnalysisTime = DateTime.Now;
        stats.TextPreview = text.Length > 30 ? text.Substring(0, 30) : text;


        // Подсчет слов и поиск самого короткого/длинного слова
        string[] words = SplitIntoWords(text);
        stats.WordCount = words.Length;

        if (words.Length > 0)
        {
            stats.ShortWord = words[0];
            stats.LongWord = words[0];

            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length < stats.ShortWord.Length)
                    stats.ShortWord = words[i];
                if (words[i].Length > stats.LongWord.Length)
                    stats.LongWord = words[i];
            }
        }

        // Подсчет предложений
        stats.SentCount = CountSentences(text);

        // Подсчет гласных, согласных и частоты букв
        CountLetters(text, stats);

        return stats;
    }
    private string[] SplitIntoWords(string text)
    {
        List<string> words = new List<string>();
        StringBuilder currentWord = new StringBuilder();
        char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}', '\t', '\n', '\r', '\"', '\'' };

        for (int i = 0; i < text.Length; i++)
        {
            char c = char.ToLower(text[i]);

            if (Array.IndexOf(separators, c) >= 0)
            {
                if (currentWord.Length > 0)
                {
                    words.Add(currentWord.ToString());
                    currentWord.Clear();
                }
            }
            else
            {
                currentWord.Append(c);
            }
        }

        // Добавляем последнее слово, если оно есть
        if (currentWord.Length > 0)
        {
            words.Add(currentWord.ToString());
        }

        return words.ToArray();
    }
    private int CountSentences(string text) // Подсчёт прелордений
    {
        int count = 0;
        bool inSentence = false;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (Array.IndexOf(sentenceSeparators, c) >= 0)
            {
                if (inSentence)
                {
                    count++;
                    inSentence = false;
                }
            }
            else if (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))
            {
                inSentence = true;
            }
        }
        // Если текст заканчивается без разделителя предложений
        if (inSentence)
        {
            count++;
        }
        return count;
    }
    private void CountLetters(string text, TextStat stats)
    {
        stats.VowelCount = 0;
        stats.ConsonantCount = 0;
        stats.LetterFrequency.Clear();

        for (int i = 0; i < text.Length; i++)
        {
            char c = char.ToLower(text[i]);

            if (char.IsLetter(c))
            {
                // Обновляем частоту букв
                if (stats.LetterFrequency.ContainsKey(c))
                {
                    stats.LetterFrequency[c]++;
                }
                else
                {
                    stats.LetterFrequency.Add(c, 1);
                }

                // Считаем гласные и согласные
                if (IsVowel(c))
                {
                    stats.VowelCount++;
                }
                else
                {
                    stats.ConsonantCount++;
                }
            }
        }

    
    }
    private bool IsVowel(char c)
    {
        for (int i = 0; i < vowels.Length; i++)
        {
            if (vowels[i] == c)
                return true;
        }
        return false;
    }



}



