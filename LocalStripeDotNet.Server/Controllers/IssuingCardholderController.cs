using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using Stripe.Issuing;
using IssuingCard = Stripe.Issuing.Card;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Controllers
{
    [ApiController]
    [Route("issuing/cardholders")]
    public class IssuingCardholderController : ControllerBase
    {
        private readonly IStripeRepository<IssuingCardholder> issuingCardholderRepository;
        private readonly IssuingCardholderGenerator issuingCardholderGenerator;

        public IssuingCardholderController(
            IStripeRepository<IssuingCardholder> issuingCardholderRepository,
            IssuingCardholderGenerator issuingCardholderGenerator)
        {
            this.issuingCardholderRepository = issuingCardholderRepository;
            this.issuingCardholderGenerator = issuingCardholderGenerator;
        }
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<IssuingCardholder> GetIssuingCard(string id)
        {
            if (!this.issuingCardholderRepository.TryGet(id, out var cardholder))
            {
                return this.NotFound();
            }

            return cardholder;
        }

        [HttpPost]
        public ActionResult<IssuingCardholder> CreateIssuingCardholder(
            [FromBody] CardholderCreateOptions cardholderCreateOptions)
        {
            var cardholder = issuingCardholderGenerator.Generate(cardholderCreateOptions);
            this.issuingCardholderRepository.Insert(cardholder);

            return this.Ok(cardholder);
        }
        
        [HttpPost]
        [Route("{id}")]
        public ActionResult<IssuingCardholder> UpdateIssuingCardholder(
            string id,
            [FromBody] CardholderUpdateOptions cardholderUpdateOptions)
        {
            if (!this.issuingCardholderRepository.TryGet(id, out var cardholder))
            {
                return this.NotFound();
            }

            var updatedCardholder = cardholder.ToUpdatedCardholder(cardholderUpdateOptions);
            this.issuingCardholderRepository.Update(updatedCardholder);

            return this.Ok(updatedCardholder);
        }
    }
}