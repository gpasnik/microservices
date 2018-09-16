using System;
using GP.Microservices.Remarks.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.Microservices.Remarks.Data
{
    public class RemarksContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Remark> Remarks { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public RemarksContext(DbContextOptions<RemarksContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasData(
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Damage"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Issue"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Suggestion"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Praise"
                    });

            modelBuilder.Entity<ActivityType>()
                .HasData(
                    new ActivityType
                    {
                        Id = Guid.NewGuid(),
                        Name = "Cleaning"
                    },
                    new ActivityType
                    {
                        Id = Guid.NewGuid(),
                        Name = "Fixing"
                    },
                    new ActivityType
                    {
                        Id = Guid.NewGuid(),
                        Name = "Activity"
                    });
        }
    }
}