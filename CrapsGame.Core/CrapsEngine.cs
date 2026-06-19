namespace CrapsGame.Core;

/// <summary>
/// Основний двигун гри Craps, який відповідає за логіку раундів, правила ставок та зміну фаз.
/// </summary>
public class CrapsEngine
{
    private readonly IDiceRoller _diceRoller;

    /// <summary>
    /// Поточна фаза гри (ComeOut або Point).
    /// </summary>
    public GamePhase CurrentPhase { get; private set; } = GamePhase.ComeOut;

    /// <summary>
    /// Встановлене очко (Point) для поточної фази гри. Рівне 0, якщо фаза ComeOut.
    /// </summary>
    public int Point { get; private set; } = 0;

    /// <summary>
    /// Ініціалізує новий екземпляр двигуна гри.
    /// </summary>
    /// <param name="diceRoller">Генератор кубиків (інтерфейс).</param>
    public CrapsEngine(IDiceRoller diceRoller)
    {
        _diceRoller = diceRoller ?? throw new ArgumentNullException(nameof(diceRoller));
    }

    /// <summary>
    /// Виконує ігровий крок: робить кидок кубиків та розраховує результат для конкретної ставки.
    /// </summary>
    /// <param name="betType">Тип ставки, яку зробив гравець.</param>
    /// <returns>Кортеж із детальними результатами кидка та фінальним статусом раунду.</returns>
    public (int Dice1, int Dice2, int Total, RoundResult Result) ExecuteStep(BetType betType)
    {
        if (betType != BetType.PassLine)
        {
            throw new NotImplementedException($"Поки тільки {nameof(BetType.PassLine)}.");
        }

        // Двигун сам викликає кубики
        var (dice1, dice2, total) = _diceRoller.Roll();

        // Розраховуємо результат залежно від поточної фази
        RoundResult result = CurrentPhase == GamePhase.ComeOut 
            ? HandleComeOutPhase(total) 
            : HandlePointPhase(total);

        return (dice1, dice2, total, result);
    }

    /// <summary>
    /// Логіка обробки першого кидка (Come-Out Roll).
    /// </summary>
    private RoundResult HandleComeOutPhase(int total)
    {
        // Натуральні числа (7 або 11) == миттєвий виграш на Pass Line
        if (total == 7 || total == 11)
        {
            ResetGame();
            return RoundResult.PlayerWin;
        }
        
        // Craps (2, 3 або 12) == миттєвий програш на Pass Line
        if (total == 2 || total == 3 || total == 12)
        {
            ResetGame();
            return RoundResult.PlayerLose;
        }

        // Будь-яке інше число (4, 5, 6, 8, 9, 10) встановлює "Point"
        Point = total;
        CurrentPhase = GamePhase.Point;
        return RoundResult.InProgress;
    }

    /// <summary>
    /// Логіка обробки кидків після встановлення очка (Point Phase).
    /// </summary>
    private RoundResult HandlePointPhase(int total)
    {
        // Якщо випав ваш Point раніше, ніж 7 == ви виграли
        if (total == Point)
        {
            ResetGame();
            return RoundResult.PlayerWin;
        }

        // Якщо випала сімка ("Seven-Out") == ви програли, раунд завершено
        if (total == 7)
        {
            ResetGame();
            return RoundResult.PlayerLose;
        }

        // Випало будь-яке інше число == раунд продовжується, потрібен наступний кидок
        return RoundResult.InProgress;
    }

    /// <summary>
    /// Скидає стан двигуна до початкових значень перед новим раундом.
    /// </summary>
    private void ResetGame()
    {
        CurrentPhase = GamePhase.ComeOut;
        Point = 0;
    }
}