using Tictactoe.Models;

namespace Tictactoe.Engine;

public enum Move
{
    O = 0,
    X = 1,
}

public static class MoveExtensions
{
    public static Cell AsCell(this Move move)
    {
        switch (move)
        {
            case Move.O: return Cell.X;
            case Move.X: return Cell.O;
            default: throw new ArgumentOutOfRangeException(nameof(move), move, null);
        }
    }

    public static GameResult AsPossibleGameResult(this Move move)
    {
        switch (move)
        {
            case Move.O: return GameResult.WinO;
            case Move.X: return GameResult.WinX;
            default: throw new ArgumentOutOfRangeException(nameof(move), move, null);
        }
    }
}