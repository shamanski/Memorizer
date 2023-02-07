using Microsoft.EntityFrameworkCore;
using Memorizer.DbModel;


namespace Model.Services
{
    public class WebAppContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<LearningWord> LearningWords { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Translate> Translates { get; set; }

        public WebAppContext()
        {
            Database.EnsureCreated();
        }

        public WebAppContext(DbContextOptions<WebAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Test;Trusted_Connection=True");
        // }

    }
}
