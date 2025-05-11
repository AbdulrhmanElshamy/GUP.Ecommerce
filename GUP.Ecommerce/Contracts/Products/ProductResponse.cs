using GUP.Ecommerce.Contracts.Category;
using System.ComponentModel.DataAnnotations.Schema;

namespace GUP.Ecommerce.Contracts.Products
{
    public class ProductResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string NameArabic { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public string SKU { get; set; } = null!;

        public CategoryResponse Category { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public bool IsFeatured { get; set; } = false;

    }
}
