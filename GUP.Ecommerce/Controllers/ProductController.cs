using GUP.Ecommerce.Abstractions.Consts;
using GUP.Ecommerce.Contracts.Common;
using GUP.Ecommerce.Contracts.Products;
using GUP.Ecommerce.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GUP.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController(IProductService service) : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters)
        {
            var result = await service.GetAllAsync(filters);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("GetByCategoryIdAsync/{categoryId}")]
        public async Task<IActionResult> GetByCategoryIdAsync([FromRoute] Guid categoryId,[FromQuery] RequestFilters filters)
        {
            var result = await service.GetByCategoryIdAsync(categoryId,filters);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("GetFeaturedProductsAsync")]
        public async Task<IActionResult> GetFeaturedProductsAsync([FromQuery] RequestFilters filters)
        {
            var result = await service.GetFeaturedProductsAsync(filters);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await service.GetByIdAsync(id);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductRequest request)
        {
            var result = await service.CreateAsync(request);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new {  result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update( [FromRoute] Guid id, [FromBody] ProductRequest request)
        {
            var result = await service.UpdateAsync(id, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus([FromRoute] Guid id)
        {
            var result = await service.ToggelStatusAsync(id);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
