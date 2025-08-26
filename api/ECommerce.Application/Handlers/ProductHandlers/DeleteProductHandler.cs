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
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cache;
        private readonly ILogger<DeleteProductHandler> _logger;
        public DeleteProductHandler(IProductRepository productRepository, ICacheService cache, ILogger<DeleteProductHandler> logger)
        {
            _productRepository = productRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ürün silme isteği alındı. ProductId: {ProductId}", request.Id);
            await _productRepository.DeleteAsync(request.Id);
            _logger.LogInformation("Ürün veritabanından silindi. ProductId: {ProductId}", request.Id);
            // cache invalidation
            await _cache.RemoveByPatternAsync("products_all_*");
            _logger.LogInformation("Ürün cache invalidation uygulandı. Key: products_all");
            
            return Unit.Value;
        }

        Task IRequestHandler<DeleteProductCommand>.Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}
