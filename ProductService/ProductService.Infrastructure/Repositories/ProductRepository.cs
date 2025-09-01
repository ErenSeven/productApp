using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Core.Entities;
using ProductService.Core.Interfaces;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _ctx;
        public ProductRepository(ProductDbContext ctx) => _ctx = ctx;

        public async Task<Product> AddAsync(Product product, CancellationToken ct = default)
        {
            _ctx.Products.Add(product);
            await _ctx.SaveChangesAsync(ct);
            return product;
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _ctx.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
            => await _ctx.Products.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(ct);

        public async Task<Product?> UpdateAsync(Product product, CancellationToken ct = default)
        {
            var existing = await _ctx.Products.FirstOrDefaultAsync(x => x.Id == product.Id, ct);
            if (existing == null) return null;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.Category = product.Category;
            existing.ImageUrl = product.ImageUrl;
            existing.UpdatedAt = DateTime.UtcNow;

            await _ctx.SaveChangesAsync(ct);
            return existing;
        }

        public async Task<bool> DeleteAsync(Product product, CancellationToken ct = default)
        {
            var existing = await _ctx.Products.FirstOrDefaultAsync(x => x.Id == product.Id, ct);
            if (existing == null) return false;

            _ctx.Products.Remove(existing);
            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IReadOnlyList<Product>> GetFilteredAsync(
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy,
            string? sortDirection,
            CancellationToken ct = default)
        {
            IQueryable<Product> query = _ctx.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category == category);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = sortBy.ToLower() switch
                {
                    "price"     => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                    "name"      => descending ? query.OrderByDescending(p => p.Name)  : query.OrderBy(p => p.Name),
                    "createdat" => descending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                    _           => query.OrderByDescending(p => p.CreatedAt) // default sıralama
                };
            }
            else
            {
                // default olarak en son eklenenler
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            return await query.ToListAsync(ct);
        }

    }
}