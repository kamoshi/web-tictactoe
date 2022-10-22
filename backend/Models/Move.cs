namespace Tictactoe.Models;

public enum Move
{
    O = 0,
    X = 1,
}

public static class MoveExtensions
{
    public static Cell AsCell(this Move move)
    {
        return move switch
        {
            Move.O => Cell.X,
            Move.X => Cell.O,
            _ => throw new ArgumentOutOfRangeException(nameof(move), move, null)
        };
    }

    public static GameState AsPossibleGameResult(this Move move)
    {
        return move switch
        {
            Move.O => GameState.WinO,
            Move.X => GameState.WinX,
            _ => throw new ArgumentOutOfRangeException(nameof(move), move, null)
        };
    }
}