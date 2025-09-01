using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Core.Events
{
    public class ProductUpdatedEvent
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public string Category { get; init; } = string.Empty;
        public string ImageUrl { get; init; } = string.Empty;
    }
}