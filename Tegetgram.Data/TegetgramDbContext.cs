using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tegetgram.Data.Entities;

namespace Tegetgram.Data
{
    public class TegetgramDbContext : IdentityDbContext<ApiUser>
    {
        public TegetgramDbContext(DbContextOptions<TegetgramDbContext> options) : base(options)
        {
        }

        public DbSet<TegetgramUser> TegetgramUsers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<UserBlocking> UserBlockings { get; set; }

        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TegetgramUser>()
                .HasMany<Message>(u => u.SentMessages)
                .WithOne(m => m.Sender)
                .HasForeignKey(u => u.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<TegetgramUser>()
                .HasMany<Message>(u => u.Inbox)
                .WithOne(m => m.Recepient)
                .HasForeignKey(u => u.RecepientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<UserBlocking>()
                .HasKey(ub => new { ub.BlockerId, ub.BlockedId });

            builder.Entity<UserBlocking>()
                .HasOne(ub => ub.Blocker)
                .WithMany(b => b.BlockingUsers)
                .HasForeignKey(ub => ub.BlockerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<UserBlocking>()
                .HasOne(ub => ub.Blocked)
                .WithMany(b => b.BlockedByUsers)
                .HasForeignKey(ub => ub.BlockedId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.OnModelCreating(builder);
        }
    }
}
