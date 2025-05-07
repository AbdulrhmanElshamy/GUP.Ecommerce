using System.ComponentModel.DataAnnotations.Schema;

namespace GUP.Ecommerce.Entities;

public class OrderItem
{
   
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; } = null!;
}


