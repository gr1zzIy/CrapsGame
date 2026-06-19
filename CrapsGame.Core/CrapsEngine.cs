namespace CrapsGame.Core;

public record StepResult(
    int Dice1, 
    int Dice2, 
    int Total, 
    GamePhase NewPhase, 
    int NewPoint,
    List<(ActiveBet Bet, BetOutcome Outcome)> EvaluatedBets
);

public class CrapsEngine
{
    private readonly IDiceRoller _diceRoller;
    public GamePhase CurrentPhase { get; private set; } = GamePhase.ComeOut;
    public int Point { get; private set; } = 0;

    public CrapsEngine(IDiceRoller diceRoller)
    {
        _diceRoller = diceRoller ?? throw new ArgumentNullException(nameof(diceRoller));
    }

    /// <summary>
    /// Робить кидок та оцінює ВСІ активні ставки на столі.
    /// </summary>
    public StepResult ExecuteStep(List<ActiveBet> activeBets)
    {
        if (activeBets == null || activeBets.Count == 0)
        {
            throw new ArgumentException("На столі має бути хоча б одна ставка.");
        }

        // 1. Кидаємо кубики
        var (d1, d2, total) = _diceRoller.Roll();

        // 2. Спочатку розраховуємо ставки, спираючись на ПОТОЧНИЙ стан столу
        var evaluatedBets = new List<(ActiveBet Bet, BetOutcome Outcome)>();
        foreach (var bet in activeBets)
        {
            var outcome = CrapsBetEvaluator.Evaluate(bet, d1, d2, total, CurrentPhase, Point);
            evaluatedBets.TupleOnIteration(bet, outcome); // Додаємо в репорт
        }

        // 3. Оновлюємо стан самого двигуна (фазу та Point)
        UpdateTableState(total);

        return new StepResult(d1, d2, total, CurrentPhase, Point, evaluatedBets);
    }

    private void UpdateTableState(int total)
    {
        if (CurrentPhase == GamePhase.ComeOut)
        {
            // Якщо не виграш/програш для PassLine, то встановлюється Point
            if (total is not (7 or 11 or 2 or 3 or 12))
            {
                Point = total;
                CurrentPhase = GamePhase.Point;
            }
        }
        else // GamePhase.Point
        {
            // Якщо випав Point або 7, раунд завершується, повертаємось до ComeOut
            if (total == Point || total == 7)
            {
                CurrentPhase = GamePhase.ComeOut;
                Point = 0;
            }
        }
    }
}

/// <summary>
/// Хелпер для зручності додавання в список
/// </summary>
static class Extensions {
    public static void TupleOnIteration(this List<(ActiveBet, BetOutcome)> list, ActiveBet b, BetOutcome o) 
        => list.Add((b, o));
}