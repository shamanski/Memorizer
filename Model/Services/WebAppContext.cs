using Microsoft.EntityFrameworkCore;
using Memorizer.DbModel;
using System.Reflection.Emit;


namespace Model.Services
{
    public class WebAppContext : DbContext  
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<LearningWord> LearningWords { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Translate> Translates { get; set; }
        public DbSet<WebAppState> CommandStates { get; set; }
        public DbSet<TelegramCode> TelegramCodes { get; set; }

        public WebAppContext()
        {
            Database.EnsureCreated();
        }

        public WebAppContext(DbContextOptions<WebAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
  => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Memorize;Trusted_Connection=True");
    }
}
