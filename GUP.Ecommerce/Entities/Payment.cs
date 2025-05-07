using GUP.Ecommerce.Entities.consts;
using System.ComponentModel.DataAnnotations.Schema;

namespace GUP.Ecommerce.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }

    public string TransactionId { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public PaymentStatus Status { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public string TapChargeId { get; set; } = null!;

    public string TapCustomerId { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public string ResponseData { get; set; } = null!;

    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; } = null!;
}


