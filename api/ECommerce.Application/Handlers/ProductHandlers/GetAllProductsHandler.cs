using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.Queries.Products;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Handlers.ProductHandlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cache;
        private readonly ILogger<GetAllProductsHandler> _logger;

        public GetAllProductsHandler(IProductRepository productRepository, ICacheService cache, ILogger<GetAllProductsHandler> logger)
        {
            _productRepository = productRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tüm ürünler sorgulandı.");
            string cacheKey = $"products_all_{request.Category ?? "all"}_" +
                            $"{request.MinPrice?.ToString() ?? "0"}_" +
                            $"{request.MaxPrice?.ToString() ?? "max"}_" +
                            $"{request.SortBy?.ToLower() ?? "name"}_" +
                            $"{request.SortDirection?.ToLower() ?? "asc"}";

            var cached = await _cache.GetAsync<IEnumerable<Product>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Ürünler cache üzerinden getirildi. Count: {Count}", cached is ICollection<Product> c ? c.Count : -1);
                return cached;
            }

            _logger.LogInformation("Cache boş, ürünler veritabanından çekiliyor...");
            
            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(request.Category))
                products = products.Where(p => p.Category.Equals(request.Category, StringComparison.OrdinalIgnoreCase));

            if (request.MinPrice.HasValue)
                products = products.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= request.MaxPrice.Value);
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                products = (request.SortBy.ToLower(), request.SortDirection?.ToLower()) switch
                {
                    ("price", "desc") => products.OrderByDescending(p => p.Price),
                    ("price", "asc") => products.OrderBy(p => p.Price),
                    ("name", "desc") => products.OrderByDescending(p => p.Name),
                    ("name", "asc") => products.OrderBy(p => p.Name),
                    _ => products.OrderBy(p => p.Name)
                };
            }
            else
            {
                products = products.OrderBy(p => p.Name);
            }
            
            products = products.ToList();

            await _cache.SetAsync(cacheKey, products, TimeSpan.FromMinutes(5));
            _logger.LogInformation("Ürünler cache'e kaydedildi. Count: {Count}", products is ICollection<Product> p ? p.Count : -1);

            return products;
        }
    }
}
