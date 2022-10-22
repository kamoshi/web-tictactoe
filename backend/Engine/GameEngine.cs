using Tictactoe.Models;

namespace Tictactoe.Engine;

public class GameEngine
{
    private readonly Cell[,] _board;
    
    public GameEngine(IReadOnlyCollection<BoardRow> rows)
    {
        _board = new Cell[rows.Count, 3];
        foreach (var (r, c, cell) in rows.SelectMany((row, r) => row.AsCells().Select((cell, c) => (r, c, cell))))
            _board[r, c] = cell;
    }

    public GameState? MakeMove(Move move, int row, int col)
    {
        var cell = _board[row, col];
        if (cell != Cell.None) return null;

        var possible = move.AsPossibleGameResult();
        _board[row, col] = move.AsCell();
        if (CheckCol(col) == possible || CheckRow(row) == possible || CheckDiagonals() == possible)
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

    public GameState CheckState()
    {
        for (var r = 0; r < _board.GetLength(0); r++) 
            switch (CheckRow(r))
            {
                case GameState.WinO: return GameState.WinO;
                case GameState.WinX: return GameState.WinX;
            }

        for (var c = 0; c < _board.GetLength(1); c++)
            switch (CheckCol(c))
            {
                case GameState.WinO: return GameState.WinO;
                case GameState.WinX: return GameState.WinX;
            }
        
        switch (CheckDiagonals())
        {
            case GameState.WinO: return GameState.WinO;
            case GameState.WinX: return GameState.WinX;
        }

        return CheckTie();
    }

    public char[][] GetBoardChars()
    {
        int rows = _board.GetLength(0), cols = _board.GetLength(1);
        var result = new char[rows][];
        for (var r = 0; r < rows; r++)
        {
            result[r] = new char[cols];
            for (var c = 0; c < cols; c++)
                result[r][c] = _board[r, c].AsChar();
        }
        return result;
    }
    
    public BoardRow[] GetBoardRows()
    {
        return _board.AsBoardRows();
    }
    
    private GameState CheckTie()
    {
        return _board.Cast<Cell>().Any(c => c == Cell.None) ? GameState.None : GameState.Tied;
    }

    private GameState CheckRow(int offsetR)
    {
        bool o = true, x = true;
        for (var c = 0; c < _board.GetLength(1); c++)
        {
            o &= Cell.O == _board[offsetR, c];
            x &= Cell.X == _board[offsetR, c];
        }
        return x ? GameState.WinX : o ? GameState.WinO : GameState.None;
    }
    
    private GameState CheckCol(int offsetC)
    {
        bool o = true, x = true;
        for (var r = 0; r < _board.GetLength(0); r++)
        {
            o &= Cell.O == _board[r, offsetC];
            x &= Cell.X == _board[r, offsetC];
        }
        return x ? GameState.WinX : o ? GameState.WinO : GameState.None;
    }

    private GameState CheckDiagonals()
    {
        int r = 0, c = 0; bool o = true, x = true;
        while (r < _board.GetLength(0) && c < _board.GetLength(1))
        {
            o &= Cell.O == _board[r, c];
            x &= Cell.X == _board[r++, c++];
        }

        if (o) return GameState.WinO;
        if (x) return GameState.WinX;

        r = 0; c = _board.GetLength(1); o = true; x = true;
        while (r < _board.GetLength(0) && c > 0)
        {
            o &= Cell.O == _board[r, --c];
            x &= Cell.X == _board[r++, c];
        }
        
        return x ? GameState.WinX : o ? GameState.WinO : GameState.None;
    }
    
}