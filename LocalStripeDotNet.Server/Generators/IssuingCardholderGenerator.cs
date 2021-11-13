using System;
using System.Collections.Generic;
using Bogus;
using Bogus.DataSets;
using Stripe.Issuing;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Generators
{
    public class IssuingCardholderGenerator {
        public static IssuingCardholder Generate(CardholderCreateOptions cardholderCreateOptions)
        {
            var issuingCardholderRule =
                new Faker<IssuingCardholder>()
                    .RuleFor(
                        o => o.Id,
                        f => StripeIdPrefixes.GetPrefix(nameof(IssuingCardholder)) + f.Random.AlphaNumeric(16))
                    .RuleFor(o => o.Object, f => "issuing.cardholder")
                    .RuleFor(
                        o => o.Billing, f => cardholderCreateOptions.Billing.ToBilling())
                    .RuleFor(o => o.Name, f => cardholderCreateOptions.Name)
                    .RuleFor(o => o.Type, cardholderCreateOptions.Type)

                    .RuleFor(o => o.Email, f => cardholderCreateOptions.Email ?? f.Internet.Email())
                    .RuleFor(o => o.PhoneNumber, f => cardholderCreateOptions.PhoneNumber ?? f.Phone.PhoneNumber())
                    .RuleFor(o => o.Livemode, f => false)
                    .RuleFor(o => o.Metadata, f => cardholderCreateOptions.Metadata ?? new Dictionary<string, string>())
                    .RuleFor(o => o.Created, DateTime.UtcNow);

            return issuingCardholderRule.Generate();
        }   
    }
}