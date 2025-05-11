namespace GUP.Ecommerce.Services.ProductStockServices
{
    public interface IProductStockService
    {
        Task<Result<List<ProductStock>>> GetAllAsync();
        Task<Result<ProductStock>> GetByIdAsync(int id);
        Task<Result<List<ProductStock>>> GetByProductIdAsync(int productId);
        Task<Result<ProductStock>> CreateAsync(ProductStock productStock);
        Task<Result<ProductStock>> UpdateAsync(int id, ProductStock productStock);
        Task<Result> DeleteAsync(int id);
        Task<Result> UpdateQuantityAsync(int productId, int quantityChange, string updatedBy);
        Task<Result<int>> GetAvailableQuantityAsync(int productId);
    }
}
