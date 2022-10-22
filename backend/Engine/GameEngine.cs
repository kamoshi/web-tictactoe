using Tictactoe.Models;

namespace Tictactoe.Engine;

public class GameEngine
{
    private readonly Cell[,] _board;
    
    public GameEngine(IReadOnlyCollection<BoardRow> rows)
    {
        _board = new Cell[rows.Count, 3];
        foreach (var (r, cells) in rows.Select((row, i) => (i, row.AsCells())))
        foreach (var (c, cell) in cells.Select((cell, i) => (i, cell)))
            _board[r, c] = cell;
    }

    public GameResult? MakeMove(Move move, int r, int c)
    {
        var cell = _board[r, c];
        if (cell != Cell.None) return null;

        var possible = move.AsPossibleGameResult();
        _board[r, c] = move.AsCell();
        if (CheckCol(c) == possible || CheckRow(r) == possible || CheckDiagonals() == possible)
            return possible;

        return CheckTie();
    }

    public Move CheckMove()
    {
        int os = 0, xs = 0;
        foreach (var cell in _board)
            switch (cell)
            {
                case Cell.O: os += 1; break;
                case Cell.X: xs += 1; break;
            }

        return os == xs ? Move.O : Move.X;
    }

    public GameResult CheckResult()
    {
        for (var r = 0; r < _board.GetLength(0); r++) 
            switch (CheckRow(r))
            {
                case GameResult.WinO: return GameResult.WinO;
                case GameResult.WinX: return GameResult.WinX;
            }

        for (var c = 0; c < _board.GetLength(1); c++)
            switch (CheckCol(c))
            {
                case GameResult.WinO: return GameResult.WinO;
                case GameResult.WinX: return GameResult.WinX;
            }
        
        switch (CheckDiagonals())
        {
            case GameResult.WinO: return GameResult.WinO;
            case GameResult.WinX: return GameResult.WinX;
        }

        return CheckTie();
    }
    
    private GameResult CheckTie()
    {
        return _board.Cast<Cell>().Any(c => c == Cell.None) ? GameResult.None : GameResult.Tied;
    }

    private GameResult CheckRow(int offsetR)
    {
        bool o = true, x = true;
        for (var c = 0; c < _board.GetLength(1); c++)
        {
            o &= Cell.O == _board[offsetR, c];
            x &= Cell.X == _board[offsetR, c];
        }
        return x ? GameResult.WinX : o ? GameResult.WinO : GameResult.None;
    }
    
    private GameResult CheckCol(int offsetC)
    {
        bool o = true, x = true;
        for (var r = 0; r < _board.GetLength(0); r++)
        {
            o &= Cell.O == _board[r, offsetC];
            x &= Cell.X == _board[r, offsetC];
        }
        return x ? GameResult.WinX : o ? GameResult.WinO : GameResult.None;
    }

    private GameResult CheckDiagonals()
    {
        int r = 0, c = 0; bool o = true, x = true;
        while (r < _board.GetLength(0) && c < _board.GetLength(1))
        {
            o &= Cell.O == _board[r, c];
            x &= Cell.X == _board[r++, c++];
        }

        if (o) return GameResult.WinO;
        if (x) return GameResult.WinX;

        r = 0; c = _board.GetLength(1); o = true; x = true;
        while (r < _board.GetLength(0) && c > 0)
        {
            o &= Cell.O == _board[r, --c];
            x &= Cell.X == _board[r++, c];
        }
        
        return x ? GameResult.WinX : o ? GameResult.WinO : GameResult.None;
    }
    
}