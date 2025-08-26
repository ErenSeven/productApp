using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using MediatR;

namespace ECommerce.Application.Queries.Products
{
    public class GetAllProductQuery : IRequest<IEnumerable<Product>>
    {
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; } 
        public string? SortDirection { get; set; }
    }
}