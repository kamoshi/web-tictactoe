using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tictactoe.Models.Entities;

public class Player
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public Player() {}

    public Player(string name)
    {
        Name = name;
    }
}
