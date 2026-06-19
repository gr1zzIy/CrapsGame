namespace CrapsGame.Core;

public static class CrapsBetEvaluator
{
    public static BetOutcome Evaluate(ActiveBet bet, int d1, int d2, int total, GamePhase phase, int point)
    {
        bool isPair = d1 == d2;

        return bet.Type switch
        {
            BetType.PassLine     => EvaluatePassLine(total, phase, point),
            BetType.DontPassLine => EvaluateDontPassLine(total, phase, point),
            BetType.Field        => EvaluateField(total),
            BetType.AnyCraps     => EvaluateAnyCraps(total),
            BetType.AnySeven     => total == 7 ? BetOutcome.Win : BetOutcome.Lose,
            BetType.YoLeven      => total == 11 ? BetOutcome.Win : BetOutcome.Lose,
            
            BetType.Hardway4     => EvaluateHardway(total, isPair, target: 4),
            BetType.Hardway6     => EvaluateHardway(total, isPair, target: 6),
            BetType.Hardway8     => EvaluateHardway(total, isPair, target: 8),
            BetType.Hardway10    => EvaluateHardway(total, isPair, target: 10),
            
            _ => throw new NotImplementedException($"Логіка для ставки {bet.Type} ще не реалізована.")
        };
    }

    private static BetOutcome EvaluatePassLine(int total, GamePhase phase, int point) => phase switch
    {
        GamePhase.ComeOut => total switch
        {
            7 or 11      => BetOutcome.Win,
            2 or 3 or 12 => BetOutcome.Lose,
            _            => BetOutcome.InProgress
        },
        GamePhase.Point => total switch
        {
            _ when total == point => BetOutcome.Win,
            7                     => BetOutcome.Lose,
            _                     => BetOutcome.InProgress
        },
        _ => BetOutcome.InProgress
    };

    private static BetOutcome EvaluateDontPassLine(int total, GamePhase phase, int point) => phase switch
    {
        GamePhase.ComeOut => total switch
        {
            2 or 3  => BetOutcome.Win,
            12      => BetOutcome.Lose, 
            7 or 11 => BetOutcome.Lose,
            _       => BetOutcome.InProgress
        },
        GamePhase.Point => total switch
        {
            7                     => BetOutcome.Win,
            _ when total == point => BetOutcome.Lose,
            _                     => BetOutcome.InProgress
        },
        _ => BetOutcome.InProgress
    };

    private static BetOutcome EvaluateField(int total) => total switch
    {
        2 or 3 or 4 or 9 or 10 or 11 or 12 => BetOutcome.Win,
        _                                  => BetOutcome.Lose
    };

    private static BetOutcome EvaluateAnyCraps(int total) => total switch
    {
        2 or 3 or 12 => BetOutcome.Win,
        _            => BetOutcome.Lose
    };

    private static BetOutcome EvaluateHardway(int total, bool isPair, int target)
    {
        // Сімка - автоматичний програш для будь-якого Hardway
        if (total == 7) 
            return BetOutcome.Lose;
        
        if (total == target)
        {
            // Випало парою (напр. 3+3) - виграш, не парою (4+2) — програш
            return isPair ? BetOutcome.Win : BetOutcome.Lose;
        }
        
        return BetOutcome.InProgress;
    }
}