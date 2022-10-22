namespace Tictactoe.Models;

public class Player
{
    public long Id { get; set;  }
    public string Name { get; set; }

    public Player(string name)
    {
        Name = name;
    }
}