using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Product;

namespace api.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductReadDto>> GetAllProductsAsync();
        Task<ProductReadDto> GetProductByIdAsync(int id);     
        Task<ProductReadDto> AddProductAsync(ProductCreateDto productCreateDto);
    }
}