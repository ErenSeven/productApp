using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Products.Commands;
using ProductService.Application.Products.Queries;
using Microsoft.AspNetCore.Authorization;

namespace ProductService.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:guid}")]
        
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection)
        {
            var query = new GetProductsQuery
            {
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories);
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand cmd)
        {
            if (id != cmd.Id)
                return BadRequest("URL'deki id ile body'deki id aynı olmalı.");

            var success = await _mediator.Send(cmd);
            return success ? NoContent() : NotFound();
        }
        
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteProductCommand { Id = id });
            return success ? NoContent() : NotFound();
        }
    }
}