using System;
using System.Collections.Generic;
using System.Text;
using CrapsGame.Core;

namespace CrapsGame.ConsoleVersion;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        IDiceRoller roller = new CryptoDiceRoller();
        CrapsEngine engine = new CrapsEngine(roller);
        List<ActiveBet> activeBets = new List<ActiveBet>();

        Console.WriteLine("--- Модуль тестування двигуна Craps ---");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"Поточна фаза: {engine.CurrentPhase}");
            if (engine.CurrentPhase == GamePhase.Point)
            {
                Console.WriteLine($"Установлене очко (Point): {engine.Point}");
            }

            PrintActiveBets(activeBets);

            Console.WriteLine("\nДоступні команди: [1] Додати ставку | [2] Кинути кубики | [3] Вихід");
            Console.Write("Введіть номер команди: ");
            string choice = Console.ReadLine()?.Trim() ?? "";

            if (choice == "1")
            {
                AddBetMenu(activeBets);
            }
            else if (choice == "2")
            {
                if (activeBets.Count == 0)
                {
                    Console.WriteLine("Помилка: для кидка потрібна хоча б одна активна ставка.");
                    continue;
                }

                ProcessRoll(engine, activeBets);
            }
            else if (choice == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Помилка: невідома команда.");
            }
        }
    }

    private static void AddBetMenu(List<ActiveBet> activeBets)
    {
        Console.WriteLine("\nВиберіть тип ставки:");
        Console.WriteLine("1. Pass Line");
        Console.WriteLine("2. Don't Pass Line");
        Console.WriteLine("3. Field");
        Console.WriteLine("4. Any Craps");
        Console.WriteLine("5. Hardway 8");
        Console.Write("Номер: ");
        
        string choice = Console.ReadLine()?.Trim() ?? "";
        BetType? selectedType = choice switch
        {
            "1" => BetType.PassLine,
            "2" => BetType.DontPassLine,
            "3" => BetType.Field,
            "4" => BetType.AnyCraps,
            "5" => BetType.Hardway8,
            _   => null
        };

        if (selectedType == null)
        {
            Console.WriteLine("Помилка: невірний вибір типу ставки.");
            return;
        }

        Console.Write("Введіть суму ставки: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Помилка: некоректна сума.");
            return;
        }

        activeBets.Add(new ActiveBet(selectedType.Value, amount));
        Console.WriteLine($"Ставку {selectedType} на суму {amount} додано.");
    }

    private static void ProcessRoll(CrapsEngine engine, List<ActiveBet> activeBets)
    {
        Console.WriteLine("\n--- Виконання кидка ---");
        var result = engine.ExecuteStep(activeBets);

        Console.WriteLine($"Результат: [{result.Dice1}] + [{result.Dice2}] = {result.Total}");

        var remainingBets = new List<ActiveBet>();

        foreach (var evaluation in result.EvaluatedBets)
        {
            var bet = evaluation.Bet;
            Console.Write($"Ставка {bet.Type} (${bet.Amount}): ");

            switch (evaluation.Outcome)
            {
                case BetOutcome.Win:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("ВИГРАШ");
                    Console.ResetColor();
                    break;

                case BetOutcome.Lose:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ПРОГРАШ");
                    Console.ResetColor();
                    break;

                case BetOutcome.InProgress:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ЗАЛИШАЄТЬСЯ В ГРІ");
                    Console.ResetColor();
                    remainingBets.Add(bet);
                    break;
            }
        }

        // Оновлюємо список лише тими ставками, які не зіграли
        activeBets.Clear();
        activeBets.AddRange(remainingBets);
    }

    private static void PrintActiveBets(List<ActiveBet> activeBets)
    {
        Console.WriteLine("Активні ставки на столі:");
        if (activeBets.Count == 0)
        {
            Console.WriteLine("  [немає ставок]");
            return;
        }

        foreach (var bet in activeBets)
        {
            Console.WriteLine($"  - {bet.Type}: {bet.Amount}");
        }
    }
}