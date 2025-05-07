using GUP.Ecommerce.Entities.consts;
using System.ComponentModel.DataAnnotations.Schema;

namespace GUP.Ecommerce.Entities;

public class Order
{
    public Guid Id { get; set; }

    public string OrderNumber { get; set; } = Guid.NewGuid().ToString().Substring(0,8);

    public string UserId { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public string ShippingAddress { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public DateTime OrderDate { get; set; } = DateTime.Now;

    public DateTime? DeliveryDate { get; set; }


    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];

    public virtual Payment Payment { get; set; } = null!;
}


