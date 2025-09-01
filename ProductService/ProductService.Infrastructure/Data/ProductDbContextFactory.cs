using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductService.Infrastructure.Data
{
    public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ProductService.API");
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var cs = config.GetConnectionString("ProductDb")
                    ?? "Host=localhost;Port=5432;Database=ProductDb;Username=postgres;Password=1234";

            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseNpgsql(cs)
                .Options;

            return new ProductDbContext(options);
        }
    }
}