using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ClevarWeb.Data.Models;
using ClevarWeb.Data.Models.Internals;
using System.Linq;
using ClevarWeb.Data.SampleData;

namespace ClevarWeb.Data
{
    public class ClevarDbContext: DefaultDbContext 
    {
        public ClevarDbContext(DbContextOptions options) : base(options)
        {
            
        }

        #region TABLES

        public DbSet<Person> People { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentData> DocumentDatas { get; set; }
        public DbSet<CartoonImage> CartoonImages { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Publication> Publications { get; set; }

        #endregion TABLES

        public void SystemHealthCheckAndSeeding()
        {
            if (!this.SystemSettings.Any())
            {
                this.Add(SystemSetting.GetDefaultSystemSetting());
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // People

            modelBuilder.Entity<Person>()
                .HasMany(c => c.CartoonImages)
                .WithOne(p => p.Person)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartoonImage>()
                .HasOne(x => x.Document);

            // Projects

            modelBuilder.Entity<Project>()
                .HasOne(x => x.Author)
                .WithMany(x => x.AuthoredProjects).IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(x => x.Supervisor)
                .WithMany(x => x.SupervisedProjects).IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasMany(x => x.AuthoredProjects)
                .WithOne(x => x.Author).IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasMany(x => x.SupervisedProjects)
                .WithOne(x => x.Supervisor).IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(x => x.PrimaryDocument);


            // Publications

            modelBuilder.Entity<Publication>()
                .HasOne(x => x.Author)
                .WithMany(x => x.AuthoredPublications).IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
