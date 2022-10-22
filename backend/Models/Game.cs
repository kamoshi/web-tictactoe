namespace Tictactoe.Models;

public class Game
{
    public long Id { get; set; }
    public GameResult Result { get; set; }
    
    // Players
    public Player? PlayerO { get; set; }
    public Player? PlayerX { get; set; }
    
    // Board
    public BoardRow Top { get; set; }
    public BoardRow Mid { get; set; }
    public BoardRow Bot { get; set; }

    public Game(Player player)
    {
        PlayerO = player;
        Top = BoardRow.Empty;
        Mid = BoardRow.Empty;
        Bot = BoardRow.Empty;
    }
}