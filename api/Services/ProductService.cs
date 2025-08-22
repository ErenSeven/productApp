using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Product;
using api.Interfaces;
using api.Mappers;

namespace api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<ProductReadDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.Select(p => p.ToProductReadDto()).ToList();
        }
    }
}