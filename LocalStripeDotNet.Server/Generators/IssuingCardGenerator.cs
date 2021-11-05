using System;
using System.Collections.Generic;
using Bogus;
using Bogus.DataSets;
using LocalStripeDotNet.Server.Repositories;
using Stripe.Issuing;
using IssuingCard = Stripe.Issuing.Card;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Generators
{
    public class IssuingCardGenerator
    {
        private readonly IStripeRepository<IssuingCardholder> issuingCardholderRepository;

        public IssuingCardGenerator(
            IStripeRepository<IssuingCardholder> issuingCardholderRepository)
        {
            this.issuingCardholderRepository = issuingCardholderRepository;
        }
        
        public IssuingCard Generate(
            CardCreateOptions cardCreateOptions,
            IssuingCardholder cardholder)
        {
            var issuingCardRule =
                new Faker<IssuingCard>()
                    .RuleFor(
                        o => o.Id, 
                        f => StripeIdPrefixes.GetPrefix(nameof(IssuingCard)) + f.Random.AlphaNumeric(16))
                    .RuleFor(o => o.Object, f => "issuing.card")
                    .RuleFor(o => o.Brand, f => "visa")
                    .RuleFor(o => o.Cardholder, f => cardholder)
                    .RuleFor(o => o.Created, f => DateTime.UtcNow)
                    .RuleFor(o => o.Currency, cardCreateOptions.Currency)
                    .RuleFor(o => o.Cvc, f => f.Finance.CreditCardCvv())
                    .RuleFor(o => o.ExpMonth, f => f.Random.Long(1, 12))
                    .RuleFor(o => o.ExpYear, f => f.Date.Future(1).Year)
                    .RuleFor(o => o.Livemode, f => false)
                    .RuleFor(o => o.Metadata, f => cardCreateOptions.Metadata ?? new Dictionary<string, string>())
                    .RuleFor(o => o.Number, 
                        (f, s) => 
                            cardCreateOptions.Type == "virtual" 
                                ? f.Finance.CreditCardNumber(CardType.Visa).Replace("-", string.Empty) 
                                : string.Empty)
                    .RuleFor(o => o.Last4, 
                        (f, s) => 
                            cardCreateOptions.Type == "virtual" 
                                ? s.Number.Substring(s.Number.Length - 4) 
                                : f.Random.Number(0, 9999).ToString("D4"))
                    .RuleFor(o => o.Status, f => cardCreateOptions.Status)
                    .RuleFor(o => o.Type, cardCreateOptions.Type);

            return issuingCardRule.Generate();
        }
    }
}