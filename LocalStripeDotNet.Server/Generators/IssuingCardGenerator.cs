using System;
using System.Collections.Generic;
using Bogus;
using Bogus.DataSets;
using LocalStripeDotNet.Server.Repositories;
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
        
        public IssuingCard Generate(string issuingCardholderId,
                                    string currency,
                                    string type,
                                    Dictionary<string, string>? metadata = null,
                                    string? status = "inactive")
        {
            var issuingCardRule =
                new Faker<IssuingCard>()
                    .RuleFor(
                        o => o.Id, 
                        f => StripeIdPrefixes.GetPrefix(nameof(IssuingCard)) + f.Random.AlphaNumeric(16))
                    .RuleFor(o => o.Object, f => "issuing.card")
                    .RuleFor(o => o.Brand, f => "visa")
                    .RuleFor(o => o.Cardholder, f => new IssuingCardholder { Id = issuingCardholderId })
                    .RuleFor(o => o.Created, f => DateTime.UtcNow)
                    .RuleFor(o => o.Currency, currency)
                    .RuleFor(o => o.Cvc, f => f.Finance.CreditCardCvv())
                    .RuleFor(o => o.ExpMonth, f => f.Random.Long(1, 12))
                    .RuleFor(o => o.ExpYear, f => f.Date.Future(1).Year)
                    .RuleFor(o => o.Livemode, f => false)
                    .RuleFor(o => o.Metadata, f => metadata)
                    .RuleFor(o => o.Number, 
                        (f, s) => 
                            type == "virtual" 
                                ? f.Finance.CreditCardNumber(CardType.Visa).Replace("-", string.Empty) 
                                : string.Empty)
                    .RuleFor(o => o.Last4, 
                        (f, s) => 
                            type == "virtual" 
                                ? s.Number.Substring(s.Number.Length - 4) 
                                : f.Random.Number(0, 9999).ToString("D4"))
                    .RuleFor(o => o.Status, f => status)
                    .RuleFor(o => o.Type, type);

            return issuingCardRule.Generate();
        }
    }
}