using GUP.Ecommerce.Abstractions;
using GUP.Ecommerce.Contracts.Category;
using GUP.Ecommerce.Contracts.Users;
using GUP.Ecommerce.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GUP.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get() 
        { 
            return Ok (await service.GetAllAsync());

        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var res = await service.GetByIdAsync(id);

            return res.IsSuccess ? Ok(res) : res.ToProblem();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var res = await service.CreateAsync(request);

            return res.IsSuccess ? CreatedAtAction(nameof(Get), new { res.Value.Id }, res.Value) : res.ToProblem();
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, CategoryRequest request)
        {
            var result = await service.UpdateAsync(id, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPost("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus( Guid id)
        {
            var result = await service.ToggelStatusAsync(id);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPost("{id}/Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await service.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

    }
}
