namespace Tictactoe.Models.Entities;

public class Player
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public Player() {}

    public Player(string name)
    {
        Name = name;
    }
}
