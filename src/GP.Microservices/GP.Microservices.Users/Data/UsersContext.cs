using System;
using GP.Microservices.Users.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.Microservices.Users.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "user1",
                        Password = "password",
                        Email = "user1-gp@mailinator.com",
                        Name = "User",
                        Lastname = "One",
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "user2",
                        Password = "password",
                        Email = "user2-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Two",
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "user3",
                        Password = "password",
                        Email = "user3-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Three",
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "user4",
                        Password = "password",
                        Email = "user4-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Four",
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "user5",
                        Password = "password",
                        Email = "user5-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Five",
                        Status = UserStatus.Active
                    }
                );
        }


        public DbSet<User> Users { get; set; }
    }
}