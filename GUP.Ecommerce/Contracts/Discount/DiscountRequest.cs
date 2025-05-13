using GUP.Ecommerce.Entities.consts;

namespace GUP.Ecommerce.Contracts.Discount
{
    public class DiscountRequest
    {

        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }


        public bool IsActive { get; set; }

        public DiscountType DiscountType { get; set; }
    }
}
