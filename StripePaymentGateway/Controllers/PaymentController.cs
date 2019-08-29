using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using StripePaymentGateway.Settings;

namespace StripePaymentGateway.Controllers
{
    public class PaymentController : Controller
    {
        private readonly StripeSettings settings;

        public PaymentController(IOptions<StripeSettings> settings)
        {
            this.settings = settings.Value;
        }

        public IActionResult Index()
        {
            ViewBag.StripePublishKey = settings.PublishableKey;
            return View();
        }
        [HttpPost]
        public ActionResult Pay(string stripeEmail, string stripeToken)
        {
            var options = new ChargeCreateOptions
            {
                Amount = 500,
                Currency = "usd",
                Description = $"Charge {stripeEmail}",
                Source = stripeToken 
            };

            var service = new ChargeService();
            Charge charge = service.Create(options);

            return Ok("Success charge");
        }
    }
}
