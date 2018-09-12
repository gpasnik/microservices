using System;
using GP.Microservices.Remarks.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GP.Microservices.Remarks.Data
{
    public class RemarksContext : DbContext
    {
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
        }
    }
}