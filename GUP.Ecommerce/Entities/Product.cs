using System.ComponentModel.DataAnnotations.Schema;

namespace GUP.Ecommerce.Entities;

public class Product : AuditableEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string NameArabic { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountPrice { get; set; }

    public string SKU { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public string ImageUrl { get; set; } = null !;

    public bool IsFeatured { get; set; } = false;

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = [];

    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
}


