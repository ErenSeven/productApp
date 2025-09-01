using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Core.Entities;

namespace ProductService.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product, CancellationToken ct = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);
        Task<Product?> UpdateAsync(Product product, CancellationToken ct = default);
        Task<bool> DeleteAsync(Product product, CancellationToken ct = default);

        Task<IReadOnlyList<Product>> GetFilteredAsync(
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy,
            string? sortDirection,
            CancellationToken cancellationToken = default);
    }
}