using GUP.Ecommerce.Contracts.Common;
using GUP.Ecommerce.Contracts.Discount;
using GUP.Ecommerce.Contracts.Products;
using GUP.Ecommerce.Services.DiscountServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GUP.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscountController(IDiscountService service) : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters)
        {
            var result = await service.GetAllAsync(filters);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await service.GetByIdAsync(id);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DiscountRequest request)
        {
            var result = await service.CreateAsync(request);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] DiscountRequest request)
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
