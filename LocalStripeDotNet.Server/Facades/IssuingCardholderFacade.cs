using System;
using System.Threading.Tasks;
using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using LocalStripeDotNet.Server.Webhooks;
using Stripe.Issuing;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Facades
{
    public class IssuingCardholderFacade
    {
        private readonly IStripeRepository<IssuingCardholder> issuingCardholderRepository;
        private readonly IWebhookInitiator webhookInitiator;

        public IssuingCardholderFacade(
            IStripeRepository<IssuingCardholder> issuingCardholderRepository,
            IWebhookInitiator webhookInitiator)
        {
            this.issuingCardholderRepository = issuingCardholderRepository;
            this.webhookInitiator = webhookInitiator;
        }

        public async Task<IssuingCardholder> CreateIssuingCardholder(CardholderCreateOptions cardholderCreateOptions)
        {
            var cardholder = IssuingCardholderGenerator.Generate(cardholderCreateOptions);
            this.issuingCardholderRepository.Insert(cardholder);

            var webhookEvent = GenerateWebhookEvent(cardholder, Stripe.Events.IssuingCardholderCreated);
            await this.webhookInitiator.InitiateWebhook(webhookEvent);

            return cardholder;
        }
        
        public async Task<IssuingCardholder> UpdateIssuingCardholder(string id, CardholderUpdateOptions cardholderUpdateOptions)
        {
            if (!this.issuingCardholderRepository.TryGet(id, out var cardholder))
            {
                throw new ArgumentException($"Cannot update non-existing Cardholder {id}");
            }

            var updatedCardholder = cardholder.ToUpdatedCardholder(cardholderUpdateOptions);
            this.issuingCardholderRepository.Update(updatedCardholder);

            var webhookEvent = GenerateWebhookEvent(cardholder, Stripe.Events.IssuingCardholderUpdated);
            await this.webhookInitiator.InitiateWebhook(webhookEvent);

            return cardholder;
        }

        public IssuingCardholder GetIssuingCardholder(string id)
        {
            if (!this.issuingCardholderRepository.TryGet(id, out var cardholder))
            {
                throw new ArgumentException($"Cannot find Cardholder {id}");
            }

            return cardholder;
        }
        
        private static StripeWebhookEvent GenerateWebhookEvent(IssuingCardholder cardholder, string eventType)
        {
            return new StripeWebhookEvent
            {
                Type = eventType,
                Data = new StripeWebhookEventData
                {
                    Object = cardholder,
                    RawObject = cardholder
                },
                Created = DateTime.UtcNow,
                Livemode = false,
            };
        }
    }
}