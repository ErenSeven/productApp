using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using MediatR;

namespace ECommerce.Application.Queries.Products
{
    public class GetProductByIdQuery : IRequest<Product?>
    {
        public Guid Id { get; set; }
    }
}
