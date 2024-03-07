using GameApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameApp.Models.Data
{
    public class GameAppDbContext : DbContext

    {
        public GameAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<GameConfiguration> GameConfigurations { get; set; }
        public DbSet<RegisteredGame> RegisteredGames { get; set; }
    }
}
