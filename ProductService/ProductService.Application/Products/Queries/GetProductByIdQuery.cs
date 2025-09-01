using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ProductService.Core.Entities;

namespace ProductService.Application.Products.Queries
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Product?>;
}