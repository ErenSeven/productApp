using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Core.Entities;

namespace ProductService.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Name).IsRequired().HasMaxLength(200);
                b.Property(p => p.Price).HasPrecision(18, 2);
                b.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
            });
        }
    }
}