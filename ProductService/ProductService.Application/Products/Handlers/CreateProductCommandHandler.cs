using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ProductService.Application.Products.Commands;
using ProductService.Core.Entities;
using ProductService.Core.Events;
using ProductService.Core.Interfaces;

namespace ProductService.Application.Products.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repo;
        private readonly IMessagePublisher _publisher;

        public CreateProductCommandHandler(IProductRepository repo, IMessagePublisher publisher)
        {
            _repo = repo;
            _publisher = publisher;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                Category = request.Category,
                ImageUrl = request.ImageUrl 
            };

            await _repo.AddAsync(product, ct);

            var evt = new ProductCreatedEvent
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                ImageUrl = product.ImageUrl 
            };
            await _publisher.PublishAsync("product.created", evt, ct);

            return product.Id;
        }
    }
}