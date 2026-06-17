namespace CrapsGame.Core;

/// <summary>
/// Інтерфейс для генератора кубиків.
/// </summary>
public interface IDiceRoller
{
    /// <summary>
    /// Кинути два кубика.
    /// </summary>
    (int Dice1, int Dice2, int Total) Roll();
}