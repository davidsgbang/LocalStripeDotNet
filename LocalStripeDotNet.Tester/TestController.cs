using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Issuing;

namespace LocalStripeDotNet.Tester
{
    [ApiController]
    [Route("test")]
    public class TestController :  ControllerBase
    {
        private readonly CardholderService cardholderService;

        public TestController()
        {
            var stripeClient = new StripeClient(
                apiKey:"123asdf",
                apiBase:"https://localhost:5001");
            this.cardholderService = new CardholderService(stripeClient);
        }
        [HttpPost]
        public async Task<Cardholder> CreateCardholder()
        {
            var options =
                new CardholderCreateOptions
                {
                    Billing = new CardholderBillingOptions
                    {
                        Address = new AddressOptions
                        {
                            Line1 = Guid.NewGuid().ToString(),
                            City = Guid.NewGuid().ToString(),
                            PostalCode = Guid.NewGuid().ToString()
                        }
                    },
                    Name = "Test",
                    Type = "individual"
                };
            var cardholder = await this.cardholderService.CreateAsync(options);
            return cardholder;
        }
    }
}