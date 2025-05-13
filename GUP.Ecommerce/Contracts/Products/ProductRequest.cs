using System.ComponentModel.DataAnnotations.Schema;

namespace GUP.Ecommerce.Contracts.Products
{
    public class ProductRequest
    {
        public string Name { get; set; } = null!;

        public string NameArabic { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public string SKU { get; set; } = null!;

        public Guid CategoryId { get; set; }

        public string? Image { get; set; } = null!;

        public bool IsFeatured { get; set; } = false;

        public virtual ICollection<string> Images{ get; set; } = [];

    }
}
