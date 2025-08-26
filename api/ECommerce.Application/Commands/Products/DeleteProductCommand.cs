using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Application.Commands.Products
{
    public class DeleteProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
