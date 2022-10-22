namespace Tictactoe.Models;

[Flags]
public enum GameResult : byte
{
    None = 0,
    Tied = 1 << 0,
    WinO = 1 << 1,
    WinX = 1 << 2,
}