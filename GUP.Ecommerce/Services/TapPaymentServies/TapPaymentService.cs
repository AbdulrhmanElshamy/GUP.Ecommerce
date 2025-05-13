using GUP.Ecommerce.Contracts.TapPayment;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using GUP.Ecommerce.Settings;

namespace GUP.Ecommerce.Services.TapPaymentServies

{
    public class TapPaymentService : ITapPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly TapSettings _tapSettings;

        public TapPaymentService(HttpClient httpClient, IOptions<TapSettings> options)
        {
            _httpClient = httpClient;
            _tapSettings = options.Value;
            _httpClient.BaseAddress = new Uri(_tapSettings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _tapSettings.SecretKey);
        }

        public async Task<Result<TapChargeResponse>> CreateChargeAsync(TapChargeRequest request)
        {
            var payload = new
            {
                amount = Math.Round(request.Amount, 3),
                currency = request.Currency,
                customer = new
                {
                    first_name = request.FirstName,
                    email = request.CustomerEmail
                },
                source = new { id = "src_all" },
                redirect = new { url = _tapSettings.RedirectUrl }
            };

            var jsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("charges", content);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Result.Failure<TapChargeResponse>(new Error("Tap payment error", $"Tap error: {responseJson}", 400));

                dynamic result = JsonConvert.DeserializeObject(responseJson);

                return Result.Success(new TapChargeResponse
                {
                    ChargeId = result.id,
                    RedirectUrl = result.transaction.url,
                    Status = result.status
                });
            }
            catch (Exception ex)
            {
                return Result.Failure<TapChargeResponse>(new Error("Some thing wrong", $"Exception occurred",500));
            }
        }

        public async Task<Result<string>> GetChargeStatusAsync(string chargeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"charges/{chargeId}");
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Result.Failure<string>(new Error("Tap Payemnt", $"Failed to get charge status: {json}",400));

                return Result.Success(json);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>((new Error("Some thing wrong", $"Exception occurred", 500)));
            }
        }
    }
}
