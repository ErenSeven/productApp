using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Product;
using api.Interfaces;
using api.Wrappers;

namespace api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound(new ApiResponse<string>("Hiç ürün bulunamadı."));
            }

            return Ok(new ApiResponse<object>(products, "Ürünler başarıyla getirildi."));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> GetProductByID(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new ApiResponse<string>($"ID {id} olan ürün bulunamadı."));

            return Ok(new ApiResponse<object>(product, "Ürün başarıyla getirildi."));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<object>), 201)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            try
            {
                var createdProduct = await _productService.AddProductAsync(productCreateDto);
                return CreatedAtAction(nameof(GetProductByID),
                    new { id = createdProduct.Id },
                    new ApiResponse<object>(createdProduct, "Ürün başarıyla eklendi."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>($"Beklenmeyen hata: {ex.Message}"));
            }
        }
    }
}