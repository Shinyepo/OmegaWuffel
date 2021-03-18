using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;

namespace OWuffel.Extensions.Database
{
    public class WuffelDBContext: DbContext
    {
        /*public WuffelDBContext(DbContextOptions<WuffelDBContext> options): base(options)
        {

        }*/
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var path = Path.Combine("Extensions", "Database", "Wuffel.db");
            options.UseSqlite($"Data Source={path}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Settings>()
                .HasIndex(c => c.guild_id)
                .IsUnique();
        }

        public DbSet<Settings> Settings { get; set; }
        public DbSet<Suggestions> Suggestions { get; set; }
    }
}
