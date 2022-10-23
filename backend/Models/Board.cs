namespace Tictactoe.Models;

[Flags]
public enum Board : ulong
{
    Empty = 0
}

public static class BoardExtensions
{
    public static Cell[,] AsCells(this Board board, int size)
    {
        var cells = new Cell[size, size];
        for (var r = 0; r < size; r++)
        for (var c = 0; c < size; c++)
            cells[r, c] = (Cell) (((ulong) board & (0b11UL << (size * r + c) * 2)) >> ((size * r + c) * 2));
        return cells;
    }
}
