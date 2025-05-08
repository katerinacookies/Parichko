using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Parichko.Models;
using DataAccess.Models;
using Parichko.Utilities;

namespace Parichko.Data
{
    public class ParichkoDbContext : DbContext
    {
        private readonly string _dbPath;
        public ParichkoDbContext()
        {
            //chat
            _dbPath = PathDb.GetPath("Parichko.db");

            
            //EnsureDatabaseExists().Wait();
        }
        /*public ParichkoDbContext(string dbPath)
        {
            _dbPath = dbPath ?? throw new ArgumentNullException(nameof(dbPath));

            //Database.EnsureCreated();
        }*/

        /*private static async Task EnsureDatabaseExists()
         {
             if (!File.Exists(_dbPath))
             {
                 //using var stream = await FileSystem.OpenAppPackageFileAsync(dbFileName);
                 using var fileStream = File.Create(_dbPath);
                 await fileStream.FlushAsync();
             }
         }*/

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //string dbPath = Path.Combine(FileSystem.AppDataDirectory, "ParichkoDb.db");
            //Console.WriteLine($"Database path: {dbPath}");
            //options.UseSqlite($"Filename={_dbPath}");
            if (string.IsNullOrEmpty(_dbPath))
            {
                throw new Exception("Database path is not set!");
            }
            //string connDb = $"Filename={PathDb.GetPath("Parichko.db")}";
            //chat
            string connDb = $"Filename={_dbPath}";
            options.UseSqlite(connDb);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //1 към 1 - 1 логин има 1 профил
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.Login)
                .WithOne(l => l.UserProfile)
                .HasForeignKey<UserProfile>(up => up.LoginId);
            //1 към много - 1 профил има много съвети, разходи
            modelBuilder.Entity<Advice>()
                .HasOne(a => a.UserProfile)
                .WithMany(up => up.Advices)
                .HasForeignKey(a => a.UserProfileId);
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.UserProfile)
                .WithMany(up => up.Expenses)
                .HasForeignKey(e => e.UserProfileId);
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId);
            modelBuilder.Entity<Income>()
                .HasOne(i => i.UserProfile)
                .WithMany(up => up.Incomes)
                .HasForeignKey(i => i.UserProfileId);
            modelBuilder.Entity<FriendRequest>()
                .HasOne(i => i.ToUser)
                .WithMany(up => up.FriendRequests)
                .HasForeignKey(i => i.ToUserId);
            //modelBuilder.Entity<UserProfile>()
            //    .HasMany(up => up.FriendRequests);
            //Много към много - 1 потребител има много цели и една цел има много потребители
            modelBuilder.Entity<UserGoal>()
                .HasKey(ug => new { ug.GoalId, ug.UserProfileId });

            modelBuilder.Entity<UserGoal>()
                .HasOne(g => g.Goal)
                .WithMany(u => u.UGoals)
                .HasForeignKey(g => g.GoalId);
            modelBuilder.Entity<UserGoal>()
                .HasOne(g => g.UserProfile)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserProfileId);
        }

        public DbSet<Advice> Advices { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<UserGoal> UserGoals { get; set; }
    }
}
