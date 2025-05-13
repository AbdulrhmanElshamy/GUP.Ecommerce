namespace GUP.Ecommerce.Contracts.TapPayment
{
    public class TapChargeRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CustomerEmail { get; set; }
        public string FirstName { get; set; }
    }

}
