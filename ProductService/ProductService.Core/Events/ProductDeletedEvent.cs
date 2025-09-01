using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Core.Events
{
    public class ProductDeletedEvent
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = string.Empty;
    }
}