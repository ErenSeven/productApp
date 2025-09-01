using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogService.Infrastructure.Data
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

        public DbSet<LogEntry> Logs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Level).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Timestamp).HasDefaultValueSql("NOW()");
            });
        }
    }
}
