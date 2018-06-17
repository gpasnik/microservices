using System;
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

            modelBuilder.Entity<Message>()
                .HasData(
                    new Message {Id = Guid.NewGuid(), Text = "message1"},
                    new Message {Id = Guid.NewGuid(), Text = "message2"});
        }

        public DbSet<Message> Messages { get; set; }
    }

    public class Message
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}