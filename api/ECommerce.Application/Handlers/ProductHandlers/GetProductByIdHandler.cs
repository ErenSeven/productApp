using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Queries.Products;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Handlers.ProductHandlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cache;
        private readonly ILogger<GetProductByIdHandler> _logger;

        public GetProductByIdHandler(IProductRepository productRepository, ICacheService cache, ILogger<GetProductByIdHandler> logger)
        {
            _productRepository = productRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ürün detay sorgulandı. ProductId: {ProductId}", request.Id);
            var cached = await _cache.GetAsync<Product?>($"product_{request.Id}");
            if (cached != null)
            {
                _logger.LogInformation("Ürün cache üzerinden getirildi. ProductId: {ProductId}", request.Id);
                return cached;
            }

            _logger.LogInformation("Cache miss. Ürün veritabanından çekiliyor... ProductId: {ProductId}", request.Id);
            var product = await _productRepository.GetByIdAsync(request.Id);
            
            if (product != null)
            {
                await _cache.SetAsync($"product_{request.Id}", product, TimeSpan.FromMinutes(5));
                _logger.LogInformation("Ürün cache'e kaydedildi. ProductId: {ProductId}", request.Id);
            }
            else
            {
                _logger.LogWarning("Ürün bulunamadı. ProductId: {ProductId}", request.Id);
            }
            return product;
        }
    }
}
