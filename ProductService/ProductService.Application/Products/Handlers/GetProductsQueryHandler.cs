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
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<Product>>
    {
        private readonly IProductRepository _repo;
        public GetProductsQueryHandler(IProductRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<Product>> Handle(GetProductsQuery request, CancellationToken ct)
        {
            return await _repo.GetFilteredAsync(
                request.Category,
                request.MinPrice,
                request.MaxPrice,
                request.SortBy,
                request.SortDirection,
                ct
            );
        }
    }
}