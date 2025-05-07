using GUP.Ecommerce.Contracts.Category;

namespace GUP.Ecommerce.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllAsync();
        Task<Result<CategoryResponse>> GetByIdAsync(Guid id);
        Task<Result<CategoryResponse>> CreateAsync(CategoryRequest category);
        Task<Result<CategoryResponse>> UpdateAsync(Guid id, CategoryRequest category);
        Task<Result> DeleteAsync(Guid id);
        Task<Result> ToggelStatusAsync(Guid id);
    }
}
