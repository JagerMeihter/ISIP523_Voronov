using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Quic;
using System.Net.WebSockets;
using System.Numerics;
using System.Text;

public static class Function
{
    public static int RollDice20()
    {
        Random random = new Random();
        return random.Next(1, 21);
        
    }

}

class Location
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public bool IsCompleted { get; set; }
    public List<string> RequiredItems { get; set; }
    public List<string> RewardItems { get; set; }

    public Location(string name, string description, string image, List<string> requiredItems = null, List<string> rewardItems = null)
    {
        Name = name;
        Description = description;
        Image = image;
        IsCompleted = false;
        RequiredItems = requiredItems ?? new List<string>();
        RewardItems = rewardItems ?? new List<string>();
    }
}
public abstract class Enemy
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }
    public int Defense { get; set; }
    public string SpecialAbility { get; set; }
    public string Weakness { get; set; }
    public double CritChance { get; set; } = 0.05; // 5% базовый шанс крита
    protected Enemy(string name)
    {
        Name = name;
    }
    public virtual int CalculateDamage(int incomingDamage, string damageType = "normal")
    {
        // Базовая формула урона с учетом защиты
        int finalDamage = incomingDamage - Defense;
        return Math.Max(finalDamage, 1); // Минимальный урон 1
    }
    public virtual int Attack()
    {
        var attackRoll = Function.RollDice20();

        return Damage;
    }

    public virtual void SpecialAttack(Player target)
    {
        // Базовая реализация для наследников
        Console.WriteLine($"{Name} использует {SpecialAbility}!");
    }

    public virtual bool IsAlive => Health > 0;
    public abstract string GetDescription();
    public void TakeDamage(int damage)
    {
        Health -= damage;
        Console.WriteLine($"{Name} получает {damage} урона. Осталось здоровья: {Health}");
    }

}
public class Player
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsFrozen { get; set; }
    public int FrozenTurns { get; set; }
    public List<string> Inventory { get; set; } = new List<string>();

    // Метод для проверки святого оружия
    public bool HasHolyWeapon()
    {
        return Inventory.Any(item =>
            item.Contains("свящ") ||
            item.Contains("святой") ||
            item.Contains("holy") ||
            item.Contains("благословен"));
    }

    public void ApplyFrozenEffect()
    {
        if (IsFrozen && FrozenTurns > 0)
        {
            Console.WriteLine("❄️ Вы заморожены и пропускаете ход!");
            FrozenTurns--;

            if (FrozenTurns == 0)
            {
                IsFrozen = false;
                Console.WriteLine("✨ Лед растаял! Вы снова можете действовать!");
            }
        }
    }
}
public class VVG : Goblinoid
{
    public VVG() : base("ВВГ", 3)
    {
        // Усиленные характеристики от обычного гоблина
        Health = 40; // ×2 от базового гоблина (60)
        MaxHealth = Health;
        Damage = 18;  // ×1.5 от базового гоблина (12)
        Defense = 6;  // ×1.2 от базового гоблина (5)
        CritChance = 0.35; // +10% от обычного гоблина (25%)
    }

    public override string GetDescription()
    {
        return $"👑 {Name} (Уровень {Level}) - Босс Гоблин\n" +
               $"❤️ Здоровье: {Health}/{MaxHealth}\n" +
               $"⚔️ Урон: {Damage}\n" +
               $"🛡️ Защита: {Defense}\n" +
               $"🎯 Крит шанс: 35%\n" +
               $"💥 Крит множитель: x2.5\n" +
               $"✨ Способность: Смертельный удар";
    }
}

public class KovalSky : Undead
{
    public KovalSky(int playerHealth) : base("Ковальский", 4)
    {
        Health = (int)(playerHealth * 2.5);
        MaxHealth = Health;
        Damage = 20;  // ×1.3 от базового скелета (15)
        Defense = 11; // ×1.4 от базового скелета (8)
    }

    public override int CalculateDamage(int incomingDamage, string damageType)
    {
        // Полностью игнорирует защиту игрока
        return incomingDamage;
    }

    public override string GetDescription()
    {
        return $"💀 {Name} (Уровень {Level}) - Босс Скелет\n" +
               $"❤️ Здоровье: {Health}/{MaxHealth}\n" +
               $"⚔️ Урон: {Damage}\n" +
               $"🛡️ Защита: {Defense} (игнорирует вашу защиту)\n" +
               $"⚠️ Слабость: Святое оружие";
    }
}

public class PestovS : Undead
{
    public PestovS(int playerHealth) : base("Пестов С--", 4)
    {
        Health = (int)(playerHealth * 1.3);
        MaxHealth = Health;
        Damage = 27;  // ×1.8 от базового скелета (15)
        Defense = 5;  // ×0.6 от базового скелета (8)
    }

    public override int Attack()
    {
        // Бросок атаки d20 + 8
        return Function.RollDice20() + 8;
    }

    public override int CalculateDamage(int incomingDamage, string damageType)
    {
        // Полностью игнорирует защиту игрока
        return incomingDamage;
    }

    public override void SpecialAttack(Player target)
    {
        var diceRoll = Function.RollDice20();
        if (diceRoll >= 17) // 15% шанс заморозки (17+ из 20)
        {
            target.IsFrozen = true;
            target.FrozenTurns = 1;
            Console.WriteLine($"❄️ {Name} замораживает вас ледяным прикосновением! Вы пропускаете ход!");
        }
        else
        {
            Console.WriteLine($"{Name} пытается заморозить вас, но вы сопротивляетесь!");
        }
    }

    public override string GetDescription()
    {
        return $"💀 {Name} (Уровень {Level}) - Босс Скелет\n" +
               $"❤️ Здоровье: {Health}/{MaxHealth}\n" +
               $"⚔️ Урон: d20 + 8\n" +
               $"🛡️ Защита: {Defense} (игнорирует вашу защиту)\n" +
               $"❄️ Шанс заморозки: 15%\n" +
               $"⚠️ Слабость: Святое оружие";
    }
}

public class ArchmageCPlusPlus : Humanoid
{
    public ArchmageCPlusPlus(int playerHealth) : base("Архимаг C++")
    {
        Health = (int)(playerHealth * 1.1);
        MaxHealth = Health;
        Damage = 0; // Использует специальную атаку
        Defense = 11; // ×1.1 от базового мага
        SpecialAbility = "❄️ Ледяная буря";
    }

    public override int Attack()
    {
        // Бросок атаки d20 + 6
        return Function.RollDice20() + 6;
    }

    public override void SpecialAttack(Player target)
    {
        var diceRoll = Function.RollDice20();
        if (diceRoll >= 18) // 10% шанс заморозки (18+ из 20)
        {
            target.IsFrozen = true;
            target.FrozenTurns = 1;
            Console.WriteLine($"❄️ {Name} накладывает на вас заморозку! Вы пропускаете ход!");
        }
        else
        {
            Console.WriteLine($"{Name} пытается заморозить вас, но заклинание не срабатывает!");
        }
    }

    public override string GetDescription()
    {
        return $"🧙 {Name} (Уровень {Level}) - Босс Маг\n" +
               $"❤️ Здоровье: {Health}/{MaxHealth}\n" +
               $"⚔️ Урон: d20 + 6\n" +
               $"🛡️ Защита: {Defense}\n" +
               $"❄️ Шанс заморозки: 10%\n" +
               $"✨ Способность: {SpecialAbility}";
    }
}
public class Humanoid : Enemy
{
    public Humanoid(string name) : base(name)
    {
        // Сбалансированные характеристики
        Health = 20;
        MaxHealth = Health;
        Damage = 8;
        Defense = 10;

        // Нет особых способностей или слабостей
        SpecialAbility = "Нет";
        Weakness = "Нет";
    }

    // Гуманоиды используют базовую логику без модификаций
    public override string GetDescription()
    {
        return $"🧍 {Name} (Уровень {Level}) - Гуманоид\n" +
               $"❤️ Здоровье: {Health}/{MaxHealth}\n" +
               $"⚔️ Урон: {Damage}\n" +
               $"🛡️ Защита: {Defense}\n" +
               $"📊 Сбалансированные характеристики";
    }
}
public class Goblinoid : Enemy
{
    private double critMultiplier;
    public Goblinoid(string name, int level) : base(name)
    {
        // Гоблиноиды имеют меньше HP но высокий шанс крита
        Health = 10;
        MaxHealth = Health;
        Damage = 12;
        Defense = 5;
        critMultiplier = 2.5; // Высокий множитель крита

        // Уникальные способности гоблиноидов
        SpecialAbility = "🎯 Смертельный удар";
        Weakness = "Низкая защита";

        // Повышенный шанс крита
        CritChance = 0.25; // 25% вместо стандартных 5%
    }
    public override int Attack()
    {
        var attackRoll = Function.RollDice20();
        int baseDamage = base.Attack();

        return baseDamage;
    }

    public override string GetDescription()
    {
        return $"🐺 {Name} (Уровень {Level}) - Гоблиноид\n" +
               $"❤️ Здоровье: {Health}/{MaxHealth}\n" +
               $"⚔️ Урон: {Damage}\n" +
               $"🛡️ Защита: {Defense}\n" +
               $"🎯 Крит шанс: 25%\n" +
               $"💥 Крит множитель: x{critMultiplier}\n" +
               $"✨ Способность: {SpecialAbility}";
    }
}
public class Undead : Enemy
{
    public Undead(string name, int level) : base(name)
    {
        Health = 80;
        MaxHealth = Health;
        Damage = 15;
        Defense = 8;

        // Уникальные способности нежити
        SpecialAbility = "❄️ Ледяное прикосновение";
        Weakness = "✨ Святое оружие";
    }

    public override int CalculateDamage(int incomingDamage, string damageType)
    {
        // Нежить получает двойной урон от святого оружия
        if (damageType == "holy")
        {
            Console.WriteLine("✨ Святая энергия сжигает нежить! Урон удвоен!");
            return incomingDamage * 2;
        }
        return base.CalculateDamage(incomingDamage, damageType);
    }
    public override void SpecialAttack(Player target)
    {
        var diceRoll = Function.RollDice20();
        base.SpecialAttack(target);
    }
    public override string GetDescription()
    {
        return $"🧟 {Name} (Уровень {Level}) - Нежить\n" +
               $"❤️ Здоровье: {Health}/{MaxHealth}\n" +
               $"⚔️ Урон: {Damage}\n" +
               $"🛡️ Защита: {Defense}\n" +
               $"✨ Способность: {SpecialAbility}\n" +
               $"⚠️ Слабость: {Weakness}";
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
    static Location[] locations = new Location[]
{
    new Location(
        "🌳 Гнилой Лес",
        "Древний лес, где деревья шепчут чужие имена. Воздух густой от запаха гниения.",
        @"
⡇⡇⠁⠀⣼⠇⢠⣷⠃⠟⠡⠏⢠⣾⣿⣿⣿⣷⣿⣿⣿⣿⣿⣿⣿⣿⣿⣗⡄⠀⠉⠀⠻⣿⣿⣦⡀⠹⣿⣷⠀⠘⣿⣿⣿⣆⠘⠐⠀⠀⠀⠀⢠⣼
⡇⡇⠀⠸⠁⢀⣿⠇⢸⢦⠃⢠⢟⣿⣿⣿⡿⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣦⡀⠀⠀⠻⣿⣷⡀⠙⢿⡇⠀⢻⢸⣿⣿⢠⠈⡅⠀⠀⠀⠀⣿
⡇⡇⠀⠀⠀⠈⡟⠀⠈⠈⢀⢼⣿⣿⣿⡿⢁⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⢹⡶⠀⠀⠈⠙⠛⢦⡀⠁⠀⠀⠘⣿⣯⠂⠀⢁⠀⠀⠀⠀⠙
⡇⠇⠀⠀⠀⠀⠃⠈⠀⠀⣼⣼⢧⣿⣿⠀⣿⣿⣿⣿⣿⠻⢰⣿⣿⣿⣿⣿⣿⣿⢷⠌⡾⡅⠀⠀⢰⡄⢀⠀⠀⠀⠀⠀⠘⢼⡆⢀⠠⠀⠀⠀⠀⠀
⠁⠀⠀⠀⠀⠀⠀⠀⠀⠠⠿⣿⣸⣿⠏⢰⡿⣿⣿⣿⠏⣄⢸⣿⣿⣿⣿⣿⣿⣿⣆⣄⢄⠃⠀⠀⠀⢿⠈⠁⢀⠀⠀⠀⠀⠈⡇⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠰⣿⠛⡛⠁⣾⠄⠈⣿⣏⣾⣿⢸⣿⣿⣿⣿⡿⣿⣿⣿⣆⢸⡧⣀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀⠐⣿⢸⢿⣿⡆⣿⡿⢛⣯⣶⣿⣿⣿⢿⡄⠃⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⠀⢸⣿⡆⢸⡟⣼⡞⣿⡇⠏⣰⣿⣿⡏⢻⣿⠯⣾⡇⢸⣿⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⢸⣿⣿⣼⠁⢸⣿⢹⡇⢠⡟⠀⡿⡀⠀⡏⣾⣿⡇⠀⣿⣿⠀⠀⠀⠀⠀⠸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⠏⠀⣿⢏⠜⠀⢻⡇⠀⣷⣿⣤⣿⡿⢻⡇⠀⢿⣿⠀⠀⠀⠀⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠛⠃⠀⠚⠇⠀⢠⠀⠘⠿⣿⣿⣿⣿⣿⡏⠈⠀⠀⠀⠛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠠⠄⢠⣽⣿⣿⣛⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣽⣾⣿⡷⠒⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠉⢙⣿⡿⠯⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣖⣀⡒⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣩⣯⣿⣿⣿⡛⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣤⡄
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠻⢿⡿⢿⣿⣿⣷⣷⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⠀⠀"
    ),

    new Location(
        "⚰️ Тихий склеп",
        "Заброшенное кладбище с склепом, из которого доносятся странные звуки.",
        @"
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠅⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣯⡙⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣴⢰⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⡗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣷⠿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⡿⠋⠁⠀⠈⠛⣿⣿⣿⠛⢿⣁⣄⣼⣿⢦⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⢢⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡘⠛⠋⠀⠀⠀⠀⠀⠀⠘⠿⣿⠄⠘⠿⠗⠓⠁⠀⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⢨⣾⣿⣿⣿⣿⣿⣿⠿⣛⡿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠘⠿⣿⡿⣿⣿⣿⣯⣭⣶⡆⣿⣿⣿⣿⣿⣿⣿⣿⡟⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠉⠻⠋⠀⠀⠉⠁⢸⣿⣿⡿⢿⠏⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠑⠀⠀⠀⠀⠀⠈⠛⣇⠀⠀⠀⣿⣿⣿⣷⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⠀⠀⠀⠙⢉⢙⣿⠉⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠐⠀⠀⡀⠀⠀⠀⠈⠠⠀⡄⠀⠀⠀⠉⠁⠆⠀⠠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠀⠐⠂⠀⠀⠀⠀⠀⠀⠀⠈⣤⣤⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢄⠒⢰⠤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⠀⠀⠀⠀⠀⠀⠀⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
"
    ),

    new Location(
        "🏰 Брошенная башня",
        "Высокая каменная башня, где когда-то жил могущественный волшебник.",
        @"
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠟⠇⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡄⠀⠀⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠿⢿⣿⣿⣿⣿
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠛⠋⠉⠀⠀⠀⠈⠉⠉⠉
⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠿⠛⠁⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀
⡿⠋⠉⠙⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠈⣿⣿⣿⣿⡙⠉⠉⠉⠉⠁⠀⠀⢀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠊⠁⠀
⠁⠀⠀⠀⠀⠈⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀⣿⣿⣿⣿⣿⣶⣾⣿⣶⡄⠀⣴⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⢠⠀⠀⠀⠀⠀⠀⠈⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀⠹⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⣠⣤⡄⠀⠀⢀⣀⣰⡦⠀
⠛⣀⡀⠀⠀⠀⠀⠀⠀⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣴⡌⠛⠉⠀⠀⠀⠀⠀⠛⠁⠈⣿⣿⣿⣿⡿⠂⠀⠀⠀⠀⠀⠀⠘⠋⠁⠀⠀⠀⠀⠉⠉⠀⠀
⠀⣿⣃⠀⠀⠀⠀⠀⠀⠀⠙⠿⢿⣿⣿⣿⣿⣿⡏⢸⣷⠀⠈⠀⠀⡀⠀⠀⠀⡀⠀⠟⡟⡉⠉⠃⠀⠀⠀⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠃⠀⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠁⠀⠙⠛⠃⠈⠃⠀⠀⠐⡋⠌⣀⠊⠀⢀⡰⠀⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠀⡐⠁⠀⠀⠈⢳⣦⠀⠀⠀⠰⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠁⠀⢤⢆⠰⠂⣤⣤⣀⣄⣠⣄⣀⠀⡀⠀⠀⣀⣴⣾⣿⠖⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠒⠒⠒⠒⠶⠶⠶⠶⠶⠤⠶⠆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠾⠷⠾⠯⣥⣀⡈⠉⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀
"
    )
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
    static void StartLocationAdventure(Location location)
    {
        Console.Clear();
        // Показываем локацию
        Console.WriteLine($"╔══════════════════════════════╗");
        Console.WriteLine($"║     {location.Name,-24} ║");
        Console.WriteLine($"╠══════════════════════════════╣");
        Console.WriteLine($"{location.Image}");

        Console.WriteLine($"╠══════════════════════════════╣");
        Console.WriteLine($" {location.Description,-28}   ");
        Console.WriteLine($"╚══════════════════════════════╝");

        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();

        // Запускаем соответствующую битву в зависимости от локации
        switch (location.Name)
        {
            case "🌳 Гнилой Лес":
                StartForestBattle();
                break;
            case "⚰️ Тихий склеп":
                StartCryptBattle();
                break;
            case "🏰 Брошенная башня":
                StartTowerBattle();
                break;
        }
    }
    static void StartTowerBattle()
    {
        Console.Clear();
        Console.WriteLine("🏰 Вы поднимаетесь в Брошенную башню...");
        System.Threading.Thread.Sleep(1500);

        // Бой с обычным магом
        var mage = new Humanoid("Маг-чародей");
        mage.Health = 70;
        mage.MaxHealth = 70;
        mage.Damage = 12;
        mage.Defense = 8;
        mage.SpecialAbility = "❄️ Ледяная стрела";

        Console.WriteLine($"\n🧙 {mage.Name} блокирует вам путь!");
        System.Threading.Thread.Sleep(1000);

        bool playerWon = CombatSystem(mage);
        if (!playerWon)
        {
            GameOver();
            return;
        }

        // Бой с боссом
        Console.WriteLine("\nПоднявшись на самый верх башни, вы встречаете Архимага C++!");
        System.Threading.Thread.Sleep(1500);

        var archmage = new ArchmageCPlusPlus(QuantityHP);
        playerWon = CombatSystem(archmage);

        if (playerWon)
        {
            Console.WriteLine("\n🎉 Вы победили Архимага и завладели башней!");
            locations[2].IsCompleted = true;
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }
        else
        {
            GameOver();
        }
    }
    static void GameOver()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║          GAME OVER           ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine("║ Ваше приключение окончено... ║");
        Console.WriteLine("║                              ║");
        Console.WriteLine("║ Ваши кости навсегда          ║");
        Console.WriteLine("║ остануться тут,может хоть    ║");
        Console.WriteLine("║ червей попитаете             ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine("\nНажмите любую клавишу чтобы вернуться в главное меню...");
        Console.ReadKey();
    }
    static bool CombatSystem(Enemy enemy)
    {
        while (enemy.IsAlive && QuantityHP > 0)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine($"║           БИТВА!            ║");
            Console.WriteLine("╠══════════════════════════════╣");
            // Показываем статус
            Console.WriteLine($"║ Ваше HP: {QuantityHP,-19} ║");
            Console.WriteLine($"║ {enemy.GetDescription().Split('\n')[0],-28} ║");
            Console.WriteLine("╠══════════════════════════════╣");

            // Ход врага
            if (!enemy.IsAlive) break;

            EnemyTurn(enemy);
            if (QuantityHP <= 0) return false;

            // Ход игрока
            if (!PlayerTurn(enemy))
                return false; // Игрок сбежал

            System.Threading.Thread.Sleep(2000);
        }

        return QuantityHP > 0;
    }
    static void EnemyTurn(Enemy enemy)
    {
        Console.WriteLine($"\n🎯 Ход {enemy.Name}:");

        var diceRoll = Function.RollDice20();
        bool useSpecial = diceRoll > 15; // 25% шанс использовать спецспособность

        if (useSpecial && enemy is Undead undead)
        {
            undead.SpecialAttack(new Player { Health = QuantityHP, Inventory = inventory });
        }
        else if (useSpecial && enemy is ArchmageCPlusPlus archmage)
        {
            archmage.SpecialAttack(new Player { Health = QuantityHP, Inventory = inventory });
        }
        else
        {
            int enemyAttack = enemy.Attack();
            Console.WriteLine($"{enemy.Name} атакует с силой {enemyAttack}!");

            // Игрок пытается блокировать
            Console.WriteLine("! У вас есть возможность заблокировать удар");
            Console.WriteLine("Нажмите любую клавишу для броска защиты...");
            Console.ReadKey();

            int defenseRoll = Function.RollDice20();
            Console.WriteLine($"🎲 Ваш бросок защиты: {defenseRoll}");

            if (defenseRoll > 10) // Успешная защита
            {
                int damage = enemy.CalculateDamage(Math.Max(0, enemyAttack - defenseRoll), "normal");
                if (damage > 0)
                {
                    QuantityHP -= damage;
                    Console.WriteLine($"💥 Вы получили {damage} урона");
                }
                else
                {
                    Console.WriteLine("🛡️ Вы отразили удар!");
                }
            }
            else
            {
                int damage = enemy.CalculateDamage(enemyAttack, "normal");
                QuantityHP -= damage;
                Console.WriteLine($"💥 Вы получили {damage} урона");
            }
        }

        // Проверяем заморозку
        if (enemy is Undead || enemy is ArchmageCPlusPlus)
        {
            var player = new Player { Health = QuantityHP, Inventory = inventory };
            player.ApplyFrozenEffect();
            if (player.IsFrozen)
            {
                Console.WriteLine("❄️ Вы заморожены и пропускаете ход!");
                return;
            }
        }
    }

    static bool PlayerTurn(Enemy enemy)
    {
        Console.WriteLine($"\n⭐ Ваш ход!");
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║         ВАШИ ДЕЙСТВИЯ        ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine("║ 1. ⚔️ Атаковать              ║");
        Console.WriteLine("║ 2. 📦 Использовать предмет   ║");
        Console.WriteLine("║ 3. 🏃 Сбежать                ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.Write("Ваш выбор: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                PlayerAttack(enemy);
                return true;
            case "2":
                ShowInventory();
                return true;
            case "3":
                Console.WriteLine("Вы пытаетесь сбежать...");
                if (Function.RollDice20() > 12)
                {
                    Console.WriteLine("✅ Вам удалось сбежать!");
                    return false;
                }
                else
                {
                    Console.WriteLine("❌ Враг не даёт вам сбежать!");
                    return true;
                }
            default:
                Console.WriteLine("Неверный выбор! Вы пропускаете ход.");
                return true;
        }
    }
    static void PlayerAttack(Enemy enemy)
    {
        int playerAttack = Function.RollDice20() + 5; // Базовая атака игрока
        Console.WriteLine($"🎲 Ваш бросок атаки: {playerAttack}");

        string damageType = "normal";
        if (enemy is Undead && inventory.Any(item => item.ToLower().Contains("свят") || item.ToLower().Contains("holy")))
        {
            damageType = "holy";
        }

        int damage = enemy.CalculateDamage(playerAttack, damageType);
        enemy.TakeDamage(damage);

        if (!enemy.IsAlive)
        {
            Console.WriteLine($"🎉 Вы победили {enemy.Name}!");
        }
    }



    static void StartCryptBattle()
    {
        Console.Clear();
        Console.WriteLine("⚰️ Вы входите в Тихий склеп...");
        System.Threading.Thread.Sleep(1500);

        // Бой с двумя скелетами
        Console.WriteLine("\n💀 Из-за саркофагов поднимаются два скелета!");
        System.Threading.Thread.Sleep(1000);

        var skeleton1 = new Undead("Скелет-воин", 2);
        var skeleton2 = new Undead("Скелет-лучник", 2);

        bool playerWon = CombatSystem(skeleton1);
        if (!playerWon)
        {
            GameOver();
            return;
        }

        Console.WriteLine("\n💀 Второй скелет атакует!");
        playerWon = CombatSystem(skeleton2);
        if (!playerWon)
        {
            GameOver();
            return;
        }

        // Бой с боссом
        Console.WriteLine("\nПосле победы над скелетами в склепе воцаряется тишина...");
        System.Threading.Thread.Sleep(1500);

        Random random = new Random();
        if (random.Next(100) < 80) // 80% шанс
        {
            Console.WriteLine("Из самого большого саркофага поднимается Ковальский!");
            var kovalSky = new KovalSky(QuantityHP);
            System.Threading.Thread.Sleep(1000);
            playerWon = CombatSystem(kovalSky);
        }
        else // 20% шанс
        {
            Console.WriteLine("Из тьмы появляется загадочный Пестов С--!");
            var pestovS = new PestovS(QuantityHP);
            System.Threading.Thread.Sleep(1000);
            playerWon = CombatSystem(pestovS);
        }

        if (playerWon)
        {
            Console.WriteLine("\n🎉 Вы очистили склеп от нежити!");
            locations[1].IsCompleted = true;
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }
        else
        {
            GameOver();
        }
    }
    static void StartForestBattle()
    {
        Console.Clear();
        Console.WriteLine("🌳 Вы входите в Гнилой Лес...");
        System.Threading.Thread.Sleep(1500);

        // Первый бой с обычным гоблином
        var goblin = new Goblinoid("Гоблин", 2);
        Console.WriteLine($"\n🐺 На вас нападает {goblin.Name}!");
        System.Threading.Thread.Sleep(1000);

        bool playerWon = CombatSystem(goblin);
        if (!playerWon)
        {
            GameOver();
            return;
        }

        // Второй бой с боссом ВВГ
        Console.WriteLine("\nПосле боя вы слышите громкий рык...");
        System.Threading.Thread.Sleep(1500);
        Console.WriteLine("Из чащи появляется огромный гоблин - ВВГ!");

        var vvg = new VVG();
        System.Threading.Thread.Sleep(1000);

        playerWon = CombatSystem(vvg);
        if (playerWon)
        {
            Console.WriteLine("\n🎉 Вы победили ВВГ и очистили лес от гоблинов!");
            locations[0].IsCompleted = true;
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }
        else
        {
            GameOver();
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

        // Переходим к меню управления вместо начала игры
        CharacterManagementMenu();
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
            Console.WriteLine("║       Инвентарь пуст         ║");
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
        Console.Clear();
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
            Console.Clear();
            Console.WriteLine("\nВаш облик:");
            Console.WriteLine(characterAppearances[Character]);
        }
    }
    static void FirstEncounter()
    {
        Console.Clear();

        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║        ВЫБОР ЛОКАЦИИ         ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine($"║ Герой: {characterName,-23}  ║");
        Console.WriteLine($"║ Предметов: {inventory.Count,-18} ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine("\nВы стоите на развилке трёх дорог. Куда направитесь?");
        Console.WriteLine();
        // Показываем доступные локации
        for (int i = 0; i < locations.Length; i++)
        {
            var location = locations[i];
            string status = location.IsCompleted ? "✅ Исследована" : "🔍 Доступна";

            Console.WriteLine($"{i + 1}. {location.Name} [{status}]");
            Console.WriteLine($"   {location.Description}");
           
            Console.WriteLine();
        }

        Console.WriteLine("4. 🔙 Вернуться в лагерь");
        Console.Write("\nВаш выбор: ");

        string choice = Console.ReadLine();

        if (int.TryParse(choice, out int locIndex) && locIndex >= 1 && locIndex <= 3)
        {
            StartLocationAdventure(locations[locIndex - 1]);
        }
        else if (choice == "4")
        {
            Console.WriteLine("\nВы возвращаетесь в безопасный лагерь...");
            System.Threading.Thread.Sleep(1500);
            CharacterManagementMenu();
        }
        else
        {
            Console.WriteLine("Нужно выбрать от 1 до 4!");
            Console.ReadKey();
            FirstEncounter();
        }
    }
    static void LocationEncounter(Location location)
    {
        Console.WriteLine($"\nВы входите в {location.Name}...");
        System.Threading.Thread.Sleep(1500);
        ContinueAdventure();
       
    }
    static void ContinueAdventure()
    {
        Console.WriteLine("\n══════════════════════════════");
        Console.WriteLine("1. 🗺️ Выбрать другую локацию");
        Console.WriteLine("2. 🏕️ Вернуться в лагерь");
        Console.WriteLine("3. 📊 Посмотреть инвентарь");
        Console.Write("\nВаш выбор: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                FirstEncounter();
                break;
            case "2":
                Console.WriteLine("\nВозвращаетесь в лагерь...");
                System.Threading.Thread.Sleep(1500);
                CharacterManagementMenu();
                break;
            case "3":
                ShowInventory();
                ContinueAdventure();
                break;
            default:
                Console.WriteLine("Возвращаю в лагерь...");
                System.Threading.Thread.Sleep(1500);
                CharacterManagementMenu();
                break;
        }
    }
    /*static void StartLocationAdventure(Location location)
    {
        Console.Clear();
        // Показываем локацию
        Console.WriteLine($"╔══════════════════════════════╗");
        Console.WriteLine($"║     {location.Name,-24} ║");
        Console.WriteLine($"╠══════════════════════════════╣");
        Console.WriteLine($"{location.Image}");
        Console.WriteLine($"╠══════════════════════════════╣");
        Console.WriteLine($"║ {location.Description,-28} ║");
        Console.WriteLine($"╚══════════════════════════════╝");

        System.Threading.Thread.Sleep(2000);

        // Запускаем ивент локации
        LocationEncounter(location);
    }*/
    static void StartAdventure()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║        НАЧАЛО ИГРЫ!          ║");
        Console.WriteLine("╠══════════════════════════════╣");
        Console.WriteLine($"║ Герой: {characterName,-23} ║");
        Console.WriteLine($"║ Предметов: {inventory.Count,-18} ║");
        Console.WriteLine("╚══════════════════════════════╝");

        Console.WriteLine("\nВы находитесь в своём тёплом лагере у костра...");
        Console.WriteLine("Ваше приключение начинается!");
        InitializeStarterInventory();
        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();

        // Здесь будет твоя основная игровая логика
        FirstEncounter();
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
