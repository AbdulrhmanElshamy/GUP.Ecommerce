using GUP.Ecommerce.Contracts.Common;
using GUP.Ecommerce.Contracts.Products;

namespace GUP.Ecommerce.Services.ProductServices
{
    public interface IProductService
    {
        Task<Result<PaginatedList<ProductResponse>>> GetAllAsync(RequestFilters filters);
        Task<Result<ProductResponse>> GetByIdAsync(Guid id);
        Task<Result<PaginatedList<ProductResponse>>> GetByCategoryIdAsync(Guid categoryId, RequestFilters filters);
        Task<Result<ProductResponse>> CreateAsync(ProductRequest product);
        Task<Result<ProductResponse>> UpdateAsync(Guid id, ProductRequest product);
        Task<Result> ToggelStatusAsync(Guid id);
        Task<Result<PaginatedList<ProductResponse>>> GetFeaturedProductsAsync(RequestFilters filters);
    }
}
