using System.ComponentModel.DataAnnotations.Schema;

namespace GUP.Ecommerce.Entities;

public class ProductStock : AuditableEntity
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid Quantity { get; set; }

    public string Location { get; set; } = null!;

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; } = null!;
}


