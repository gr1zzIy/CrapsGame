namespace CrapsGame.Core;

/// <summary>
/// Модель активної ставки гравця
/// </summary>
/// <param name="Type">Тип ставки</param>
/// <param name="Amount">Сума ставки</param>
public record ActiveBet(BetType Type, decimal Amount);