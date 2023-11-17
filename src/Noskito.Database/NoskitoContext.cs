using Microsoft.EntityFrameworkCore;
using Noskito.Database.Entity;

namespace Noskito.Database
{
    public class NoskitoContext : DbContext
    {
        public DbSet<DbAccount> Accounts { get; set; }
        public DbSet<DbCharacter> Characters { get; set; }
        public DbSet<DbMap> Maps { get; set; }
        public DbSet<DbMonster> Monsters { get; set; }
        public DbSet<DbMonsterData> MonstersData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=noskito;Username=noskito;Password=noskito");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbAccount>()
                .HasMany(x => x.Characters)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);

            modelBuilder.Entity<DbMap>()
                .HasMany(x => x.Monsters)
                .WithOne(x => x.Map)
                .HasForeignKey(x => x.MapId);

            modelBuilder.Entity<DbMonsterData>()
                .HasMany(x => x.Monsters)
                .WithOne(x => x.Data)
                .HasForeignKey(x => x.GameId);
        }
    }
}