using System.Diagnostics;

namespace Tictactoe.Models;

public enum Cell
{
    None = 0,
    O = 1,
    X = 2,
}

public static class CellExtensions
{
    public static char AsChar(this Cell cell)
    {
        return cell switch
        {
            Cell.None => ' ',
            Cell.O => 'O',
            Cell.X => 'X',
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null)
        };
    }

    public static BoardRow[] AsBoardRows(this Cell[,] cells)
    {
        var rows = cells.GetLength(0);
        var cols = cells.GetLength(1);
        var boardRows = new BoardRow[rows];
        for (var r = 0; r < rows; r++)
        {
            var row = new BoardRow();
            for (var c = 0; c < cols; c++)
            {
                row |= (BoardRow) (cells[r, c] switch
                {
                    Cell.O => 1 << c * 2,
                    Cell.X => 1 << c * 2 + 1,
                    _ => 0
                });
            }
            boardRows[r] = row;
        }
        return boardRows;
    }
}
