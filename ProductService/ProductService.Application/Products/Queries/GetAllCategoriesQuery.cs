using MediatR;
using System.Collections.Generic;

namespace ProductService.Application.Products.Queries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<string>>
    {
    }
}
