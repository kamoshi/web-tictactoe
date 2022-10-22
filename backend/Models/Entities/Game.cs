namespace Tictactoe.Models.Entities;

public class Game
{
    public long Id { get; set; }
    public GameState State { get; set; }
    
    // Players
    public Player? PlayerO { get; set; }
    public Player? PlayerX { get; set; }
    
    // Board
    public BoardRow Top { get; set; }
    public BoardRow Mid { get; set; }
    public BoardRow Bot { get; set; }
    
    public Game() {}

    public Game(Player player)
    {
        PlayerO = player;
        Top = BoardRow.Empty;
        Mid = BoardRow.Empty;
        Bot = BoardRow.Empty;
    }
}