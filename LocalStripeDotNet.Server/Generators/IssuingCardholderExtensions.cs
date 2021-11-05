using System.Linq;
using Stripe;
using Stripe.Issuing;

namespace LocalStripeDotNet.Server.Generators
{
    public static class IssuingCardholderExtensions
    {
        public static CardholderBilling ToBilling(this CardholderBillingOptions cbo)
        {
            return new CardholderBilling
            {
                Address = new Address
                {
                    Line1 = cbo.Address.Line1,
                    Line2 = cbo.Address.Line2,
                    City = cbo.Address.City,
                    Country = cbo.Address.Country,
                    PostalCode = cbo.Address.PostalCode,
                    State = cbo.Address.State,
                }
            };
        }

        public static Cardholder ToUpdatedCardholder(this Cardholder cardholder, CardholderUpdateOptions options)
        {
            if (options.Billing?.Address != null)
            {
                cardholder.Billing = options.Billing.ToBilling();
            }

            if (options.Company?.TaxId != null)
            {
                cardholder.Company.TaxIdProvided = true;
            }

            if (!string.IsNullOrEmpty(options.Email))
            {
                cardholder.Email = options.Email;
            }

            if (options.Status != null)
            {
                cardholder.Status = options.Status;
            }

            if (!string.IsNullOrEmpty(options.PhoneNumber))
            {
                cardholder.PhoneNumber = options.PhoneNumber;
            }

            if (options.Metadata != null)
            {
                options.Metadata.ToList().ForEach(x => cardholder.Metadata[x.Key] = x.Value);
            }

            return cardholder;
        }
    }
}