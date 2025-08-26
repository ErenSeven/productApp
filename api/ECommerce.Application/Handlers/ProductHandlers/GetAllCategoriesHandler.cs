using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Queries.Products;
using ECommerce.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Handlers.ProductHandlers
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetAllCategoriesHandler> _logger;

        public GetAllCategoriesHandler(IProductRepository productRepository, ILogger<GetAllCategoriesHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => p.Category)
                            .Where(c => !string.IsNullOrEmpty(c))
                            .Distinct().ToList();
        }
    }
}