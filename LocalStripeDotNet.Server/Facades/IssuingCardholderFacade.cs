using System;
using System.Threading.Tasks;
using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using LocalStripeDotNet.Server.Webhooks;
using Stripe;
using Stripe.Issuing;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Facades
{
    public class IssuingCardholderFacade
    {
        private readonly IStripeRepository<IssuingCardholder> issuingCardholderRepository;
        private readonly IWebhookInitiator webhookInitiator;
        private readonly IssuingCardholderGenerator issuingCardholderGenerator;

        public IssuingCardholderFacade(
            IStripeRepository<IssuingCardholder> issuingCardholderRepository,
            IWebhookInitiator webhookInitiator,
            IssuingCardholderGenerator issuingCardholderGenerator)
        {
            this.issuingCardholderRepository = issuingCardholderRepository;
            this.webhookInitiator = webhookInitiator;
            this.issuingCardholderGenerator = issuingCardholderGenerator;
        }

        public async Task<IssuingCardholder> CreateIssuingCardholder(CardholderCreateOptions cardholderCreateOptions)
        {
            var cardholder = issuingCardholderGenerator.Generate(cardholderCreateOptions);
            this.issuingCardholderRepository.Insert(cardholder);

            var webhookEvent = GenerateWebhookEvent(cardholder, Stripe.Events.IssuingCardholderCreated);
            await this.webhookInitiator.InitiateWebhook(webhookEvent);

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