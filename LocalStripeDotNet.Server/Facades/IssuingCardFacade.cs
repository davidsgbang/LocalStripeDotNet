using System;
using System.Threading.Tasks;
using LocalStripeDotNet.Server.Controllers;
using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using LocalStripeDotNet.Server.Webhooks;
using Stripe;
using Stripe.Issuing;
using CardCreateOptions = Stripe.Issuing.CardCreateOptions;
using CardUpdateOptions = Stripe.Issuing.CardUpdateOptions;
using IssuingCard = Stripe.Issuing.Card;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Facades
{
    public class IssuingCardFacade
    {
        private readonly IStripeRepository<IssuingCard> issuingCardRepository;
        private readonly IStripeRepository<IssuingCardholder> issuingCardholderRepository;
        private readonly IWebhookInitiator webhookInitiator;

        public IssuingCardFacade(
            IStripeRepository<IssuingCard> issuingCardRepository, 
            IStripeRepository<IssuingCardholder> issuingCardholderRepository, 
            IWebhookInitiator webhookInitiator)
        {
            this.issuingCardRepository = issuingCardRepository;
            this.issuingCardholderRepository = issuingCardholderRepository;
            this.webhookInitiator = webhookInitiator;
        }
        
        public IssuingCard GetIssuingCard(string id)
        {
            if (!this.issuingCardRepository.TryGet(id, out var card))
            {
                throw new ArgumentException($"Cannot find IssuingCard with id {id}");
            }

            return card;
        }

        public async Task<IssuingCard> CreateIssuingCard(CardCreateOptions cardCreateOptions)
        {
            if (!this.issuingCardholderRepository.TryGet(cardCreateOptions.Cardholder, out var cardholder)) {
                throw new ArgumentException("Cannot create IssuingCard without existing Cardholder");
            }
            
            var generatedCard = IssuingCardGenerator.Generate(cardCreateOptions, cardholder);
            this.issuingCardRepository.Insert(generatedCard);
            
            var webhookEvent = GenerateWebhookEvent(generatedCard, Stripe.Events.IssuingCardCreated);
            await this.webhookInitiator.InitiateWebhook(webhookEvent);

            return generatedCard;
        }
        
        
        public async Task<IssuingCard> UpdateIssuingCard(string id, CardUpdateOptions cardUpdateOptions)
        {
            if (!this.issuingCardRepository.TryGet(id, out var card))
            {
                throw new ArgumentException($"Cannot update non-existing Card {id}");
            }

            var updatedCard = card.ToUpdatedCard(cardUpdateOptions);
            this.issuingCardRepository.Update(updatedCard);

            var webhookEvent = GenerateWebhookEvent(updatedCard, Stripe.Events.IssuingCardUpdated);
            await this.webhookInitiator.InitiateWebhook(webhookEvent);

            return updatedCard;
        }
        
        private static StripeWebhookEvent GenerateWebhookEvent(IssuingCard issuingCard, string eventType)
        {
            return new StripeWebhookEvent
            {
                Type = eventType,
                Data = new StripeWebhookEventData
                {
                    Object = issuingCard,
                    RawObject = issuingCard
                },
                Created = DateTime.UtcNow,
                Livemode = false,
            };
        }
    }
}