using Memorizer.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Model.Entities;

namespace Model.Services
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {

        public IdentityDbContext()
        {
        }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
  => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Identity;Trusted_Connection=True");
    }
}
