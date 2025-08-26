using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Commands.Products;
using ECommerce.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Handlers.ProductHandlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cache;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(IProductRepository productRepository, ICacheService cache, ILogger<UpdateProductHandler> logger)
        {
            _productRepository = productRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ürün güncelleme isteği alındı. ProductId: {ProductId}", request.Id);
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                _logger.LogWarning("Güncellenecek ürün bulunamadı. ProductId: {ProductId}", request.Id);
                throw new Exception("Product not found");
            }

            if (!string.IsNullOrEmpty(request.Name)) product.Name = request.Name;
            if (!string.IsNullOrEmpty(request.Description)) product.Description = request.Description;
            if (request.Price.HasValue) product.Price = request.Price.Value;
            if (!string.IsNullOrEmpty(request.Category)) product.Category = request.Category;
            if (!string.IsNullOrEmpty(request.ImageUrl)) product.ImageUrl = request.ImageUrl;
            if (request.Stock.HasValue) product.Stock = request.Stock.Value;

            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            _logger.LogInformation("Ürün başarıyla güncellendi. ProductId: {ProductId}", request.Id);
            // cache invalidation
            await _cache.RemoveByPatternAsync("products_all_*");
            _logger.LogInformation("Ürün listesi cache'i temizlendi. ProductId: {ProductId}", request.Id);
            return Unit.Value;
        }

        Task IRequestHandler<UpdateProductCommand>.Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}