using GUP.Ecommerce.Contracts.Common;
using GUP.Ecommerce.Contracts.Products;
using GUP.Ecommerce.Entities;
using GUP.Ecommerce.Helpers;
using System.Linq.Dynamic.Core;

namespace GUP.Ecommerce.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Result<PaginatedList<ProductResponse>>> GetAllAsync(RequestFilters filters)
        {


            var query = _context.Products
                .AsQueryable();


            if (!string.IsNullOrEmpty(filters.SearchValue))
            {
                query = query.Where(x => x.Name.Contains(filters.SearchValue) || x.NameArabic.Contains(filters.SearchValue));
            }

            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");
            }

            var source = query
                            .Include(p => p.Category)
                            .ProjectToType<ProductResponse>()
                            .AsNoTracking();

            var products = await PaginatedList<ProductResponse>.CreateAsync(source, filters.PageNumber, filters.PageSize);

            return Result.Success(products);

        }

        public async Task<Result<ProductResponse>> GetByIdAsync(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return Result.Failure<ProductResponse>(ProductErrors.ProductNotFound);

            return Result.Success(product.Adapt<ProductResponse>());
        }

        public async Task<Result<PaginatedList<ProductResponse>>> GetByCategoryIdAsync(Guid categoryId, RequestFilters filters)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return Result.Failure<PaginatedList<ProductResponse>>(CategoryErrors.CategoryNotFound);

            var query = _context.Products
                .Where(p => p.CategoryId == categoryId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filters.SearchValue))
            {
                query = query.Where(x => x.Name.Contains(filters.SearchValue) || x.NameArabic.Contains(filters.SearchValue));
            }

            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");
            }

            var source = query
                            .Include(p => p.Category)
                            .ProjectToType<ProductResponse>()
                            .AsNoTracking();

            var products = await PaginatedList<ProductResponse>.CreateAsync(source, filters.PageNumber, filters.PageSize);

            return Result.Success(products);
        }

        public async Task<Result<ProductResponse>> CreateAsync(ProductRequest request)
        {

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                return Result.Failure<ProductResponse>(CategoryErrors.CategoryNotFound);

            var product = request.Adapt<Product>();

            if (string.IsNullOrEmpty(request.Image))
                return Result.Failure<ProductResponse>(ProductErrors.ProducImageRequired);


            product.ImageUrl = FileUploader.SaveImage(request.Image, _webHostEnvironment.ContentRootPath);
            if (string.IsNullOrEmpty(product.ImageUrl))
                return Result.Failure<ProductResponse>(ProductErrors.ProducImageInvalid);


            if(request.Images.Any())
                foreach (var item in request.Images)
                    product.ProductImages.Add(new ProductImage { ImageUrl = FileUploader.SaveImage(item, _webHostEnvironment.ContentRootPath)});



            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Result.Success(product.Adapt<ProductResponse>());
        }

        public async Task<Result<ProductResponse>> UpdateAsync(Guid id, ProductRequest request)
        {

            var existingProduct = await _context.Products.Include(c => c.ProductImages).FirstOrDefaultAsync(c=> c.Id == id);
            if (existingProduct == null)
                return Result.Failure<ProductResponse>(ProductErrors.ProductNotFound);

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                return Result.Failure<ProductResponse>(CategoryErrors.CategoryNotFound);

            existingProduct.Name = request.Name;
            existingProduct.NameArabic = request.NameArabic;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;
            existingProduct.SKU = request.SKU;
            existingProduct.CategoryId = request.CategoryId;
            existingProduct.IsFeatured = request.IsFeatured;

            if (!string.IsNullOrEmpty(request.Image))
            {
                FileUploader.DeleteImage(existingProduct.ImageUrl, _webHostEnvironment.WebRootPath);
                existingProduct.ImageUrl = FileUploader.SaveImage(request.Image, _webHostEnvironment.WebRootPath);
                if (string.IsNullOrEmpty(existingProduct.ImageUrl))
                    return Result.Failure<ProductResponse>(ProductErrors.ProducImageInvalid);
            }

            if (existingProduct.ProductImages.Any())
                foreach (var item in existingProduct.ProductImages)
                FileUploader.DeleteImage(item.ImageUrl, _webHostEnvironment.WebRootPath);

            if (request.Images.Any())
                foreach (var item in request.Images)
                    existingProduct.ProductImages.Add(new ProductImage { ImageUrl = FileUploader.SaveImage(item, _webHostEnvironment.ContentRootPath) });


            await _context.SaveChangesAsync();
            return Result.Success(existingProduct.Adapt<ProductResponse>());
        }

        public async Task<Result> ToggelStatusAsync(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.OrderItems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return Result.Failure(ProductErrors.ProductNotFound);

            if (product.OrderItems != null && product.OrderItems.Any())
                return Result.Failure(ProductErrors.CannotDeleteProduct);

            product.IsDeleted = !product.IsDeleted;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<PaginatedList<ProductResponse>>> GetFeaturedProductsAsync(RequestFilters filters)
        {

            var query = _context.Products
                .Where(c => c.IsFeatured)
                .AsQueryable();


            if (!string.IsNullOrEmpty(filters.SearchValue))
            {
                query = query.Where(x => x.Name.Contains(filters.SearchValue) || x.NameArabic.Contains(filters.SearchValue));
            }

            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");
            }

            var source = query
                            .Include(p => p.Category)
                            .ProjectToType<ProductResponse>()
                            .AsNoTracking();

            var products = await PaginatedList<ProductResponse>.CreateAsync(source, filters.PageNumber, filters.PageSize);

            return Result.Success(products);
        }
    }
}
