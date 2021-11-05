using LocalStripeDotNet.Server.Controllers.Requests;
using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using IssuingCard = Stripe.Issuing.Card;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Controllers
{
    [ApiController]
    [Route("issuing/cards")]
    public class IssuingCardController : ControllerBase
    {
        private readonly IStripeRepository<IssuingCard> issuingCardRepository;
        private readonly IssuingCardGenerator issuingCardGenerator;

        public IssuingCardController(
            IStripeRepository<IssuingCard> issuingCardRepository,
            IssuingCardGenerator issuingCardGenerator)
        {
            this.issuingCardRepository = issuingCardRepository;
            this.issuingCardGenerator = issuingCardGenerator;
        }
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<IssuingCard> GetIssuingCard(string id)
        {
            if (!this.issuingCardRepository.TryGet(id, out var card))
            {
                return this.NotFound();
            }

            return card;
        }

        [HttpPost]
        public ActionResult<IssuingCard> CreateIssuingCard(
            [FromBody] CardCreateOptions cardCreateOptions)
        {
            var x = this.issuingCardGenerator.Generate(
                cardCreateOptions.Cardholder,
                cardCreateOptions.Currency,
                cardCreateOptions.Type,
                cardCreateOptions.Metadata,
                cardCreateOptions.Status!.ToString());

            return this.Ok(x);
        }
    }
}