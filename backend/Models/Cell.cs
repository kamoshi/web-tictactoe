namespace Tictactoe.Models;

[Flags]
public enum Cell : byte
{
    Empty = 0,
    O = 1 << 0,
    X = 1 << 1,
    Both = 1 << 2,
}

public static class CellExtensions
{
    public static char AsChar(this Cell cell)
    {
        return cell switch
        {
            Cell.Empty => ' ',
            Cell.O => 'O',
            Cell.X => 'X',
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null)
        };
    }

    public static Board AsBoard(this Cell[,] cells)
    {
        var size = cells.GetLength(0);
        var board = Board.Empty;
        for (var r = 0; r < size; r++)
        for (var c = 0; c < size; c++)
            board |= (Board) ((byte) cells[r, c] << ((size * r + c) * 2));
        return board;
    }
}
