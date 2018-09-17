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
                        Id = Guid.Parse("C5261772-D5FF-4588-AFF2-735A1E5C01B6"),
                        Name = "Damage"
                    },
                    new Category
                    {
                        Id = Guid.Parse("B8F48F0F-7E26-4DA4-A28F-38A5C646A08C"),
                        Name = "Issue"
                    },
                    new Category
                    {
                        Id = Guid.Parse("276F404B-A545-4A71-BFD8-15BFE32E8F3F"),
                        Name = "Suggestion"
                    },
                    new Category
                    {
                        Id = Guid.Parse("ADB13C39-2B03-4FDA-A222-E72C9388C2AC"),
                        Name = "Praise"
                    });

            modelBuilder.Entity<ActivityType>()
                .HasData(
                    new ActivityType
                    {
                        Id = Guid.Parse("EB3B4815-0600-4701-BD7B-36852E43699D"),
                        Name = "Cleaning"
                    },
                    new ActivityType
                    {
                        Id = Guid.Parse("DBF89A9C-E817-4FAB-8B50-37F8E7A02B69"),
                        Name = "Fixing"
                    },
                    new ActivityType
                    {
                        Id = Guid.Parse("BEFCE9A8-A323-44A9-BC28-AD013CCE5264"),
                        Name = "Activity"
                    });
        }
    }
}