using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProductService.Core.Interfaces;
using ProductService.Core.Events;
using ProductService.Application.Products.Commands; 

namespace ProductService.Application.Products.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repo;
        private readonly IMessagePublisher _publisher;

        public UpdateProductCommandHandler(IProductRepository repo, IMessagePublisher publisher)
        {
            _repo = repo;
            _publisher = publisher;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(request.Id, ct);
            if (product == null) return false;

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.Category = request.Category;
            product.ImageUrl = request.ImageUrl;
            product.UpdatedAt = DateTime.UtcNow;
            
            await _repo.UpdateAsync(product, ct);

            var updatedEvent = new ProductUpdatedEvent
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                ImageUrl = product.ImageUrl
            };

            await _publisher.PublishAsync("product.updated", updatedEvent, ct);
            
            return true;
        }
    }
}
