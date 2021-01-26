using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace OWuffel.Extensions.Database
{
    public class WuffelDBContext: DbContext
    {
        /*public WuffelDBContext(DbContextOptions<WuffelDBContext> options): base(options)
        {

        }*/
        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite("Data Source=Extensions\\Database\\Wuffel.db");
       
        public DbSet<Settings> Settings { get; set; }
    }
}
