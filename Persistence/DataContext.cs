using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<League> Leagues { get; set; }
    }
}
