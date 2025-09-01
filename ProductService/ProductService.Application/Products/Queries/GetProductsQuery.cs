using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ProductService.Core.Entities;

namespace ProductService.Application.Products.Queries
{
    public class GetProductsQuery : IRequest<IReadOnlyList<Product>>
    {
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } // "asc" | "desc"
    }
}