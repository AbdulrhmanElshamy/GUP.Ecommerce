using GUP.Ecommerce.Contracts.Common;
using GUP.Ecommerce.Contracts.Discount;
using System.Linq.Dynamic.Core;

namespace GUP.Ecommerce.Services.DiscountServices
{
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _context;

        public DiscountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PaginatedList<DiscountResponse>>> GetAllAsync(RequestFilters filters)
        {


            var query = _context.Discounts
                .AsQueryable();


            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");
            }

            var source = query
                            .ProjectToType<DiscountResponse>()
                            .AsNoTracking();

            var discounts = await PaginatedList<DiscountResponse>.CreateAsync(source, filters.PageNumber, filters.PageSize);

            return Result.Success(discounts);

        }

        public async Task<Result<DiscountResponse>> GetByIdAsync(Guid id)
        {
            var discount = await _context.Discounts
                .FirstOrDefaultAsync(p => p.Id == id);

            if (discount == null)
                return Result.Failure<DiscountResponse>(DiscountErrors.DiscountNotFound);

            return Result.Success(discount.Adapt<DiscountResponse>());
        }

        public async Task<Result<DiscountResponse>> CreateAsync(DiscountRequest request)
        {

            if (request.DiscountType == Entities.consts.DiscountType.Percentage && (request.Amount > 100 || request.Amount <= 0))
                return Result.Failure<DiscountResponse>(DiscountErrors.DiscountNotValidAmount);

            if (request.DiscountType == Entities.consts.DiscountType.Amount && request.Amount < 0)
                return Result.Failure<DiscountResponse>(DiscountErrors.DiscountNotValid);


            if (request.StartDate > request.EndDate)
                return Result.Failure<DiscountResponse>(DiscountErrors.DiscountNotValidDate);


            var discount = request.Adapt<Discount>();

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return Result.Success(discount.Adapt<DiscountResponse>());
        }

        public async Task<Result<DiscountResponse>> UpdateAsync(Guid id, DiscountRequest request)
        {

            if (request.DiscountType == Entities.consts.DiscountType.Percentage && (request.Amount > 100 || request.Amount <= 0))
                return Result.Failure<DiscountResponse>(DiscountErrors.DiscountNotValidAmount);

            if (request.DiscountType == Entities.consts.DiscountType.Amount && request.Amount < 0)
                return Result.Failure<DiscountResponse>(DiscountErrors.DiscountNotValid);

            if(request.StartDate > request.EndDate)
                return Result.Failure<DiscountResponse>(DiscountErrors.DiscountNotValidDate);


            var existingDiscount = await _context.Discounts.FirstOrDefaultAsync(c => c.Id == id);
            if (existingDiscount == null)
                return Result.Failure<DiscountResponse>(DiscountErrors.DiscountNotFound);

            existingDiscount.StartDate = request.StartDate;
            existingDiscount.EndDate = request.EndDate;
            existingDiscount.Amount = request.Amount;
            existingDiscount.DiscountType = request.DiscountType;
            existingDiscount.IsActive = request.IsActive;

            await _context.SaveChangesAsync();
            return Result.Success(existingDiscount.Adapt<DiscountResponse>());
        }

        public async Task<Result> ToggelStatusAsync(Guid id)
        {
            var discount = await _context.Discounts
                .FirstOrDefaultAsync(p => p.Id == id);

            if (discount == null)
                return Result.Failure(DiscountErrors.DiscountNotFound);

           
            discount.IsActive = !discount.IsActive;

            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

    }
}
