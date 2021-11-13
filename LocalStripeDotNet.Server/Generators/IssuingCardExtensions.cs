using System.Linq;
using Stripe.Issuing;

namespace LocalStripeDotNet.Server.Generators
{
    public static class IssuingCardExtensions
    {
        public static Card ToUpdatedCard(this Card card, CardUpdateOptions options)
        {
            if (options.CancellationReason != null)
            {
                card.CancellationReason = options.CancellationReason;
            }

            if (!string.IsNullOrEmpty(options.Status))
            {
                card.Status = options.Status;
            }

            if (options.Metadata != null)
            {
                options.Metadata.ToList().ForEach(x => card.Metadata[x.Key] = x.Value);
            }

            return card;
        }
    }
}