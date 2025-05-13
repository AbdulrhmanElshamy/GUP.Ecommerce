using GUP.Ecommerce.Contracts.Common;
using GUP.Ecommerce.Contracts.Discount;
using GUP.Ecommerce.Contracts.Products;

namespace GUP.Ecommerce.Services.DiscountServices
{
    public interface IDiscountService
    {
        Task<Result<PaginatedList<DiscountResponse>>> GetAllAsync(RequestFilters filters);
        Task<Result<DiscountResponse>> GetByIdAsync(Guid id);
        Task<Result<DiscountResponse>> CreateAsync(DiscountRequest product);
        Task<Result<DiscountResponse>> UpdateAsync(Guid id, DiscountRequest product);
        Task<Result> ToggelStatusAsync(Guid id);
    }
}
