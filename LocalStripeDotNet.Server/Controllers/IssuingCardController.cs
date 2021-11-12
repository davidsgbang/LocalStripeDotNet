using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using LocalStripeDotNet.Server.Webhooks;
using Microsoft.AspNetCore.Mvc;
using Stripe.Issuing;
using CardCreateOptions = Stripe.Issuing.CardCreateOptions;
using IssuingCard = Stripe.Issuing.Card;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Controllers
{
    [ApiController]
    [Route("issuing/cards")]
    public class IssuingCardController : ControllerBase
    {
        private readonly IStripeRepository<IssuingCard> issuingCardRepository;
        private readonly IStripeRepository<IssuingCardholder> issuingCardholderRepository;
        private readonly IWebhookInitiator webhookInitiator;
        private readonly IssuingCardGenerator issuingCardGenerator;

        public IssuingCardController(
            IStripeRepository<IssuingCard> issuingCardRepository,
            IStripeRepository<IssuingCardholder> issuingCardholderRepository,
            IWebhookInitiator webhookInitiator,
            IssuingCardGenerator issuingCardGenerator)
        {
            this.issuingCardRepository = issuingCardRepository;
            this.issuingCardholderRepository = issuingCardholderRepository;
            this.webhookInitiator = webhookInitiator;
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
            if (!this.issuingCardholderRepository.TryGet(cardCreateOptions.Cardholder, out var cardholder)) {
                return this.BadRequest();
            }
            
            var generatedCard = this.issuingCardGenerator.Generate(cardCreateOptions, cardholder);

            this.issuingCardRepository.Insert(generatedCard);

            return this.Ok(generatedCard);
        }
        
        [HttpPost]
        [Route("{id}")]
        public ActionResult<IssuingCard> UpdateIssuingCard(
            string id,
            [FromBody] CardUpdateOptions cardCreateOptions)
        {
            if (!this.issuingCardRepository.TryGet(id, out var card)) {
                return this.BadRequest();
            }

            var updatedCard = card.ToUpdatedCardholder(cardCreateOptions);

            this.issuingCardRepository.Update(updatedCard);

            return this.Ok(updatedCard);
        }
    }
}