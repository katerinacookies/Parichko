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

namespace Parichko.Data
{
    public class ParichkoDbContext : DbContext
    {
        private static string dbFileName = "ParichkoDb.db";
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName);

        public ParichkoDbContext()
        {
            EnsureDatabaseExists().Wait();
        }

        private static async Task EnsureDatabaseExists()
        {
            if (!File.Exists(dbPath))
            {
                //using var stream = await FileSystem.OpenAppPackageFileAsync(dbFileName);
                using var fileStream = File.Create(dbPath);
                await fileStream.FlushAsync();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //string dbPath = Path.Combine(FileSystem.AppDataDirectory, "ParichkoDb.db");
            //Console.WriteLine($"Database path: {dbPath}");
            options.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //1 към 1 - 1 логин има 1 профил
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.Login)
                .WithOne(l => l.UserProfile)
                .HasForeignKey<UserProfile>(up => up.Id);
            //1 към много - 1 профил има много съвети, разходи
            modelBuilder.Entity<Advice>()
                .HasOne(a => a.UserProfile)
                .WithMany(up => up.Advices)
                .HasForeignKey(a => a.UserProfileId);
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.UserProfile)
                .WithMany(up => up.Expenses)
                .HasForeignKey(e => e.UserProfileId);
            modelBuilder.Entity<Income>()
                .HasOne(i => i.UserProfile)
                .WithMany(up => up.Incomes)
                .HasForeignKey(i => i.UserProfileId);
            modelBuilder.Entity<FriendRequest>()
                .HasOne(i => i.ToUser)
                .WithMany(up => up.FriendRequests)
                .HasForeignKey(i => i.ToUserId);
            //Много към много - 1 потребител има много цели и една цел има много потребители
            modelBuilder.Entity<UserGoal>()
                .HasKey(ug => new { ug.GoalId, ug.UserProfileId });

            modelBuilder.Entity<UserGoal>()
                .HasOne(g => g.Goal)
                .WithMany(u => u.Savers)
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
    }
}
