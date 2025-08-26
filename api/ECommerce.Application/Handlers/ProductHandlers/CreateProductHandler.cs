using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Commands.Products;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Handlers.ProductHandlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cache;
        
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(IProductRepository productRepository, ICacheService cache, ILogger<CreateProductHandler> logger)
        {
            _productRepository = productRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yeni ürün oluşturma isteği alındı. Name: {Name}, Category: {Category}", 
                request.Name, request.Category);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                ImageUrl = request.ImageUrl,
                Stock = request.Stock
            };

            await _productRepository.AddAsync(product);
            _logger.LogInformation("Ürün veritabanına kaydedildi. ProductId: {ProductId}", product.Id);
            // cache invalidation
            await _cache.RemoveByPatternAsync("products_all_*");
            _logger.LogInformation("Ürün cache invalidation uygulandı. Key: products_all");

            return product.Id;
        }
    }
}