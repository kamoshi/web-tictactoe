using Microsoft.EntityFrameworkCore;

namespace Tictactoe.Configuration;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=Database.db");
    }
    
    // Models
    public DbSet<Models.Entities.Player>? Player { get; set; }
    public DbSet<Models.Entities.Game>? Game { get; set; }
}
