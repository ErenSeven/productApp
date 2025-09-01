using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ProductService.Application.Products.Queries;
using ProductService.Core.Entities;
using ProductService.Core.Interfaces;

namespace ProductService.Application.Products.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository _repo;
        public GetProductByIdQueryHandler(IProductRepository repo) => _repo = repo;

        public Task<Product?> Handle(GetProductByIdQuery request, CancellationToken ct)
            => _repo.GetByIdAsync(request.Id, ct);
    }
}