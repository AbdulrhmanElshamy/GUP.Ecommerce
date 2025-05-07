using GUP.Ecommerce.Contracts.Category;
using System.Collections.Generic;

namespace GUP.Ecommerce.Services.CategoryServices
{

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Adapt<List<CategoryResponse>>();
        }

        public async Task<Result<CategoryResponse>> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound);

            return Result.Success(category.Adapt<CategoryResponse>());
        }

        public async Task<Result<CategoryResponse>> CreateAsync(CategoryRequest category)
        {

            await _context.Categories.AddAsync(category.Adapt<Category>());
            await _context.SaveChangesAsync();

            return Result.Success(category.Adapt<CategoryResponse>());
        }

        public async Task<Result<CategoryResponse>> UpdateAsync(Guid id, CategoryRequest category)
        {

            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory is null)
                return Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound);

            existingCategory.Name = category.Name;
            existingCategory.NameArabic = category.NameArabic;

            await _context.SaveChangesAsync();
            return Result.Success(existingCategory.Adapt<CategoryResponse>());
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                return Result.Failure(CategoryErrors.CategoryNotFound);

            if (category.Products != null && category.Products.Any())
                return Result.Failure(CategoryErrors.CannotDeleteCategory);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Result.Success();
        }


        public async Task<Result> ToggelStatusAsync(Guid id)
        {
            var category = await _context.Categories
              .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                return Result.Failure(CategoryErrors.CategoryNotFound);

            category.IsDeleted = !category.IsDeleted;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
