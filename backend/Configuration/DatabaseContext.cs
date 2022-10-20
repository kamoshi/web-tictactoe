using Microsoft.EntityFrameworkCore;
using Tictactoe.Models;

namespace Tictactoe.Configuration;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=Database.db");
    }
    
    public DbSet<Tictactoe.Models.Player>? Player { get; set; }
}
