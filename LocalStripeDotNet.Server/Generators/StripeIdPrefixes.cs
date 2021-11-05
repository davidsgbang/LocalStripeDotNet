using System.Collections.Generic;
using Stripe;
using Stripe.Issuing;
using Card = Stripe.Card;
using Dispute = Stripe.Dispute;
using IssuingCard = Stripe.Issuing.Card;
using IssuingCardholder = Stripe.Issuing.Cardholder;

namespace LocalStripeDotNet.Server.Generators
{
    public static class StripeIdPrefixes
    {
        private static readonly Dictionary<string, string> Prefixes = 
                new Dictionary<string, string>
                {
                    {nameof(Account), "acct_"},
                    {nameof(Charge), "ch_"},
                    {nameof(Customer), "cus_"},
                    {nameof(Dispute), "dp_"},
                    {nameof(Event), "evt_"},
                    {nameof(File), "file_"},
                    {nameof(SetupIntent), "seti_"},
                    {nameof(PaymentIntent), "pi_"},
                    {nameof(Payout), "po_"},
                    {nameof(Refund), "re_"},
                    {nameof(Token), "tok_"},
                    {nameof(PaymentMethod), "pm_"},
                    {nameof(BankAccount), "ba_"},
                    {nameof(Card), "card_"},
                    {nameof(Source), "src_"},
                    {nameof(CreditNote), "cn_"},
                    {nameof(CustomerBalanceTransaction), "cbtxn_"},
                    {nameof(Invoice), "in_"},
                    {nameof(InvoiceItem), "ii_"},
                    {nameof(Product), "prod_"},
                    {nameof(Subscription), "sub_"},
                    {nameof(IssuingCard), "ic_"},
                    {nameof(IssuingCardholder), "ich_"},
                    {nameof(Transaction), "txn_"},
                };

        public static string GetPrefix(string type)
        {
            return Prefixes[type];
        } 
    }
}