namespace CrapsGame.Core;

/// <summary>
/// Фази раунду в грі Craps.
/// </summary>
public enum GamePhase
{
    /// <summary>
    /// Початкова фаза раунду (перший кидок кубиків).
    /// </summary>
    ComeOut,
    
    /// <summary>
    /// Фаза встановленого очка (повторні кидки до випадіння Point або 7).
    /// </summary>
    Point,
}

/// <summary>
/// Поточний статус або фінальний результат поточного раунду.
/// </summary>
public enum RoundResult
{
    /// <summary>
    /// Раунд ще не завершено, потрібен наступний кидок кубиків.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Раунд завершено перемогою гравця.
    /// </summary>
    PlayerWin,
    
    /// <summary>
    /// Раунд завершено програшем гравця (перемогою казино).
    /// </summary>
    PlayerLose
}

/// <summary>
/// Типи ставок, доступні гравцеві на столі Craps.
/// </summary>
public enum BetType
{
    /// <summary>
    /// Ставка на те, що стрілець (гравець) виграє. Базова ставка в грі.
    /// </summary>
    PassLine,
    
    /// <summary>
    /// Ставка проти стрільця (на те, що виграє казино). По суті, протилежність Pass Line.
    /// </summary>
    DontPassLine,
}