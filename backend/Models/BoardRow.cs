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
