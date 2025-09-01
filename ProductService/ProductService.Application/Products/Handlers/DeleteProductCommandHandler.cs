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
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repo;
        private readonly IMessagePublisher _publisher;

        public DeleteProductCommandHandler(IProductRepository repo, IMessagePublisher publisher)
        {
            _repo = repo;
            _publisher = publisher;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(request.Id, ct);
            if (product == null) return false;

            await _repo.DeleteAsync(product, ct);

            var evt = new ProductDeletedEvent
            {
                Id = product.Id,
                Name = product.Name
            };
            await _publisher.PublishAsync("product.deleted", evt, ct);

            return true;
        }
    }
}
