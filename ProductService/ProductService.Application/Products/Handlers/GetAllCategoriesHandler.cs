using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Products.Queries;
using ProductService.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Handlers
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
            var products = await _productRepository.GetAllAsync(cancellationToken);

            return products.Select(p => p.Category)
                           .Where(c => !string.IsNullOrWhiteSpace(c))
                           .Distinct()
                           .ToList();
        }
    }
}
