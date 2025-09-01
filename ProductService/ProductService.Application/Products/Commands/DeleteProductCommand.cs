using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MediatR;

namespace ProductService.Application.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public Guid Id { get; init; }
    }
}
