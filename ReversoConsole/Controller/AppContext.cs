﻿using Microsoft.EntityFrameworkCore;
using ReversoConsole.DbModel;

namespace ReversoConsole.Controller
{
    class AppContext: DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<Translate> Translates { get; set; }
      //  public DbSet<Phrase> Phrases { get; set; }
        public AppContext()
        {
          //  Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Test;Trusted_Connection=True") ;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Translate>().HasIndex(u => u.Text).IsUnique();
        }
    }
}