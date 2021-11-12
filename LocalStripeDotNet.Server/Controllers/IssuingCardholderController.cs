using System.Threading.Tasks;
using LocalStripeDotNet.Server.Facades;
using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using Stripe.Issuing;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Controllers
{
    [ApiController]
    [Route("issuing/cardholders")]
    public class IssuingCardholderController : ControllerBase
    {
        private readonly IStripeRepository<IssuingCardholder> issuingCardholderRepository;
        private readonly IssuingCardholderFacade issuingCardholderFacade;

        public IssuingCardholderController(
            IStripeRepository<IssuingCardholder> issuingCardholderRepository,
            IssuingCardholderFacade issuingCardholderFacade)
        {
            this.issuingCardholderRepository = issuingCardholderRepository;
            this.issuingCardholderFacade = issuingCardholderFacade;
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
        public async Task<ActionResult<Cardholder>> CreateIssuingCardholder(
            [FromBody] CardholderCreateOptions cardholderCreateOptions)
        {
            var cardholder = 
                await this.issuingCardholderFacade.CreateIssuingCardholder(cardholderCreateOptions);

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