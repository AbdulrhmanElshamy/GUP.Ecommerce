using Azure;
using GUP.Ecommerce.Contracts.TapPayment;
using GUP.Ecommerce.Services.TapPaymentServies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace GUP.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly ITapPaymentService _tapService;

        public PaymentController(ITapPaymentService tapService)
        {
            _tapService = tapService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] TapChargeRequest request)
        {
            var response = await _tapService.CreateChargeAsync(request);
            return response.IsSuccess ? Ok(response) : response.ToProblem();
        }

        [HttpGet("status/{chargeId}")]
        public async Task<IActionResult> GetPaymentStatus(string chargeId)
        {
            var status = await _tapService.GetChargeStatusAsync(chargeId);
            return status.IsSuccess ? Ok(JsonConvert.DeserializeObject(status.Value!)) : status.ToProblem();
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> TapWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            Console.WriteLine("Webhook received: " + body);

            dynamic data = JsonConvert.DeserializeObject(body);

            string eventType = data.type;
            string chargeId = data.id;
            string status = data.status;

            var signature = Request.Headers["tap-signature"];
            bool isValid = IsValidSignature(body, signature, "your-webhook-secret-key");
            if (!isValid)
                return Unauthorized();


            if (eventType == "CHARGE.SUCCEEDED")
            {
                // Mark the order as paid in your database
            }
            else if (eventType == "CHARGE.FAILED")
            {
                // Mark the order as failed or notify the user
            }

            return Ok(); // Respond with 200 to confirm receipt
        }

        private bool IsValidSignature(string body, string signature, string secret)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(body);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string computed = BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
                return computed == signature;
            }
        }
    }
}
