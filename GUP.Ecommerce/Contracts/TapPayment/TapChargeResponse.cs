namespace GUP.Ecommerce.Contracts.TapPayment
{
    public class TapChargeResponse
    {
        public string ChargeId { get; set; }
        public string RedirectUrl { get; set; }
        public string Status { get; set; }
    }

}
