using Tictactoe.Engine;

namespace Tictactoe.Models;

[Flags]
public enum BoardRow : byte
{
    Lo = 1 << 0,
    Lx = 1 << 1,
    Mo = 1 << 2,
    Mx = 1 << 3,
    Ro = 1 << 4,
    Rx = 1 << 5,
}

public static class BoardRowExtensions
{
    public static Cell[] AsCells(this BoardRow row)
    {
        var cells = new[] {Cell.None, Cell.None, Cell.None};
        
        if (row.HasFlag(BoardRow.Lo)) cells[0] = Cell.O;
        else if (row.HasFlag(BoardRow.Lx)) cells[0] = Cell.X;

        if (row.HasFlag(BoardRow.Mo)) cells[1] = Cell.O;
        else if (row.HasFlag(BoardRow.Mx)) cells[1] = Cell.X;

        if (row.HasFlag(BoardRow.Ro)) cells[2] = Cell.O;
        else if (row.HasFlag(BoardRow.Rx)) cells[2] = Cell.X;

        return cells;
    }
}
