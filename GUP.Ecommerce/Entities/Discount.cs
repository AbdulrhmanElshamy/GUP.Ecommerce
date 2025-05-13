using GUP.Ecommerce.Entities.consts;

namespace GUP.Ecommerce.Entities;

public class Discount
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime StartDate { get; set; }


    public DateTime EndDate { get; set; }


    public bool IsActive { get; set; }

    public DiscountType DiscountType { get; set; }
}
