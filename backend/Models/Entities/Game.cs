using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tictactoe.Models.Entities;

public class Game
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public long Id { get; set; }
    
    public GameState State { get; set; }
    
    // Players
    public Player? PlayerO { get; set; }
    public Player? PlayerX { get; set; }
    
    // Board
    public byte Size = 3;
    public Board Board { get; set; }
    
    public Game() {}

    public Game(Player player)
    {
        PlayerO = player;
        Board = Board.Empty;
    }
}