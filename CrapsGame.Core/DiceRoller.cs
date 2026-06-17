using System.Security.Cryptography;

namespace CrapsGame.Core;

public static class DiceRoller
{
    /// <summary>
    /// Імітація кидання двох кубиків та повернення результату
    /// </summary>
    /// <returns>Метод повертає кортеж (tuple) з двох кубиків та їх суми</returns>
    public static (int Dice1, int Dice2, int Total) Roll()
    {
        int dice1 = GetCryptoRandomNumber(1, 7);
        int dice2 = GetCryptoRandomNumber(1, 7);
        int total = dice1 + dice2;
        return (dice1, dice2, total);
    }

    /// <summary>
    /// Генерує безпечне число в діапазоні [min, max)
    /// </summary>
    /// <param name="min">Початок діапазону</param>
    /// <param name="max">Кінець діапазону</param>
    /// <returns>Число з діапазону</returns>
    private static int GetCryptoRandomNumber(int min, int max)
    {
        return RandomNumberGenerator.GetInt32(min, max);
    }
}