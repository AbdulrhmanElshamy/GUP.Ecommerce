using GUP.Ecommerce.Contracts.TapPayment;

namespace GUP.Ecommerce.Services.TapPaymentServies
{
    public interface ITapPaymentService
    {
        Task<Result<TapChargeResponse>> CreateChargeAsync(TapChargeRequest request);
        Task<Result<string>> GetChargeStatusAsync(string chargeId);
    }
}
