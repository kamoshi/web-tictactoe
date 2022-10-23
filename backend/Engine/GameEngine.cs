using Tictactoe.Models;

namespace Tictactoe.Engine;

public class GameEngine
{
    private readonly Cell[,] _cells;
    public Board Board => _cells.AsBoard();
    
    public GameEngine(Board board, int size)
    {
        _cells = board.AsCells(size);
    }

    public GameState? MakeMove(Move move, int row, int col)
    {
        if (move != CheckMove())
            return null;
        var cell = _cells[row, col];
        if (cell != Cell.Empty)
            return null;
        _cells[row, col] = move.AsCell();
        return CheckState();
    }

    public Move CheckMove()
    {
        int os = 0, xs = 0;
        foreach (var cell in _cells)
            switch (cell)
            {
                case Cell.O: os += 1; break;
                case Cell.X: xs += 1; break;
            }

        return os == xs ? Move.O : Move.X;
    }

    public GameState CheckState()
    {
        for (var r = 0; r < _cells.GetLength(0); r++) 
            switch (CheckRow(r))
            {
                case GameState.WinO: return GameState.WinO;
                case GameState.WinX: return GameState.WinX;
            }

        for (var c = 0; c < _cells.GetLength(1); c++)
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
        int rows = _cells.GetLength(0), cols = _cells.GetLength(1);
        var result = new char[rows][];
        for (var r = 0; r < rows; r++)
        {
            result[r] = new char[cols];
            for (var c = 0; c < cols; c++)
                result[r][c] = _cells[r, c].AsChar();
        }
        return result;
    }

    private GameState CheckTie()
    {
        return _cells.Cast<Cell>().Any(c => c == Cell.Empty) ? GameState.None : GameState.Tied;
    }

    private GameState CheckRow(int offsetR)
    {
        bool o = true, x = true;
        for (var c = 0; c < _cells.GetLength(1); c++)
        {
            o &= Cell.O == _cells[offsetR, c];
            x &= Cell.X == _cells[offsetR, c];
        }
        return x ? GameState.WinX : o ? GameState.WinO : GameState.None;
    }
    
    private GameState CheckCol(int offsetC)
    {
        bool o = true, x = true;
        for (var r = 0; r < _cells.GetLength(0); r++)
        {
            o &= Cell.O == _cells[r, offsetC];
            x &= Cell.X == _cells[r, offsetC];
        }
        return x ? GameState.WinX : o ? GameState.WinO : GameState.None;
    }

    private GameState CheckDiagonals()
    {
        int r = 0, c = 0; bool o = true, x = true;
        while (r < _cells.GetLength(0) && c < _cells.GetLength(1))
        {
            o &= Cell.O == _cells[r, c];
            x &= Cell.X == _cells[r++, c++];
        }

        if (o) return GameState.WinO;
        if (x) return GameState.WinX;

        r = 0; c = _cells.GetLength(1); o = true; x = true;
        while (r < _cells.GetLength(0) && c > 0)
        {
            o &= Cell.O == _cells[r, --c];
            x &= Cell.X == _cells[r++, c];
        }
        
        return x ? GameState.WinX : o ? GameState.WinO : GameState.None;
    }
    
}