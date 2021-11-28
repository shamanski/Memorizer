using Microsoft.EntityFrameworkCore;
using ReversoConsole.DbModel;

namespace ReversoConsole.Controller
{
    class AppContext: DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<Word> Translates { get; set; }
        public DbSet<Word> Phrases { get; set; }
        public AppContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Test;Trusted_Connection=True") ;
        }
    }
}
