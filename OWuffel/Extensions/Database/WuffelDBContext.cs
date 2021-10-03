using Microsoft.EntityFrameworkCore;
using OWuffel.Models;
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
            //var path = Path.Combine("Extensions", "Database", "Wuffel.db");
            //options.UseSqlite($"Data Source={path}");
            options.UseNpgsql("Server=192.168.1.14;Port=5454;Database=OWuffel;User Id=OWuffelDashboard;Password=p@Zaj0swUbr;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BotSettings>()
                .HasOne(x => x.GuildInformation)
                .WithOne(x => x.BotSettings)
                .HasForeignKey<BotSettings>(x => x.GuildId)
                .HasPrincipalKey<GuildInformation>(x => x.GuildId);

            modelBuilder.Entity<DailyRankingConfig>()
                .HasOne(x=>x.GuildInformation)
                .WithOne(x=>x.DailyRankingConfig)
                .HasForeignKey<DailyRankingConfig>(x=>x.GuildId)
                .HasPrincipalKey<GuildInformation>(x=>x.GuildId);

            
            modelBuilder.Entity<LogsConfig>()
                .HasOne(x=>x.GuildInformation)
                .WithOne(x=>x.LogsConfig)
                .HasForeignKey<LogsConfig>(x=>x.GuildId)
                .HasPrincipalKey<GuildInformation>(x=>x.GuildId);

            modelBuilder.Entity<ReactionsConfig>()
                .HasOne(x => x.GuildInformation)
                .WithOne(x => x.ReactionsConfig)
                .HasForeignKey<ReactionsConfig>(x => x.GuildId)
                .HasPrincipalKey<GuildInformation>(x => x.GuildId);

            modelBuilder.Entity<Suggestions>()
                .HasOne(x=>x.GuildInformation)
                .WithOne(x=>x.Suggestions)
                .HasForeignKey<Suggestions>(x=>x.GuildId)
                .HasPrincipalKey<GuildInformation>(x=>x.GuildId);
            
            modelBuilder.Entity<SuggestionsConfig>()
                .HasOne(x=>x.GuildInformation)
                .WithOne(x=>x.SuggestionsConfig)
                .HasForeignKey<SuggestionsConfig>(x=>x.GuildId)
                .HasPrincipalKey<GuildInformation>(x=>x.GuildId);
            
            modelBuilder.Entity<SupportConfig>()
                .HasOne(x=>x.GuildInformation)
                .WithOne(x=>x.SupportConfig)
                .HasForeignKey<SupportConfig>(x=>x.GuildId)
                .HasPrincipalKey<GuildInformation>(x=>x.GuildId);
            
            modelBuilder.Entity<Tickets>()
                .HasOne(x=>x.GuildInformation)
                .WithOne(x=>x.Tickets)
                .HasForeignKey<Tickets>(x=>x.GuildId)
                .HasPrincipalKey<GuildInformation>(x=>x.GuildId);
            
            modelBuilder.Entity<WelcomeMessageConfig>()
                .HasOne(x=>x.GuildInformation)
                .WithOne(x=>x.WelcomeMessageConfig)
                .HasForeignKey<WelcomeMessageConfig>(x=>x.GuildId)
                .HasPrincipalKey<GuildInformation>(x=>x.GuildId);
            
            
        }

        public DbSet<BotSettings> BotSettings { get; set; }
        public DbSet<DailyRankingConfig> DailyRankingConfigs { get; set; }
        public DbSet<GuildInformation> GuildInformations { get; set; }
        public DbSet<LogsConfig> LogsConfigs { get; set; }
        public DbSet<ReactionsConfig> ReactionsConfigs { get; set; }
        public DbSet<SuggestionsConfig> SuggestionsConfigs { get; set; }
        public DbSet<Suggestions> Suggestions { get; set; }
        public DbSet<SupportConfig> SupportConfigs { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<WelcomeMessageConfig> WelcomeMessageConfigs { get; set; }




    }
}
