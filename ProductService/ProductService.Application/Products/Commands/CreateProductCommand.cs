using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ProductService.Application.Products.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public string Category { get; init; } = string.Empty;
        public string ImageUrl { get; init; } = string.Empty;
    }
}