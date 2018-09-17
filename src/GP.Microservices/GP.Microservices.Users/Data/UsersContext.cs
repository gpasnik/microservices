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
                        Id = Guid.Parse("7749874B-DFDD-46E9-AAA8-F2B02CBD19B2"),
                        Username = "user1",
                        Password = "password",
                        Email = "user1-gp@mailinator.com",
                        Name = "User",
                        Lastname = "One",
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = Guid.Parse("F3DE0DEF-67D9-49F4-AAC5-E24661941E78"),
                        Username = "user2",
                        Password = "password",
                        Email = "user2-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Two",
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = Guid.Parse("96C50DB1-6F47-48EC-A15E-998388FE1C9B"),
                        Username = "user3",
                        Password = "password",
                        Email = "user3-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Three",
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = Guid.Parse("B940D0AC-E3BE-4482-B853-EB3FBD7D4E53"),
                        Username = "user4",
                        Password = "password",
                        Email = "user4-gp@mailinator.com",
                        Name = "User",
                        Lastname = "Four",
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = Guid.Parse("D5AF270D-58CF-4FDF-BDF3-ACAFA65BA018"),
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