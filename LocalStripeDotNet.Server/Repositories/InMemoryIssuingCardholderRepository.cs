using Stripe.Issuing;

namespace LocalStripeDotNet.Server.Repositories
{
    public class InMemoryIssuingCardholderRepository : IStripeRepository<Cardholder>
    {
        void IStripeRepository<Cardholder>.Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        void IStripeRepository<Cardholder>.Flush()
        {
            throw new System.NotImplementedException();
        }

        void IStripeRepository<Cardholder>.Insert(Cardholder record)
        {
            throw new System.NotImplementedException();
        }

        bool IStripeRepository<Cardholder>.TryGet(string id, out Cardholder record)
        {
            throw new System.NotImplementedException();
        }

        Cardholder IStripeRepository<Cardholder>.Update(Cardholder record)
        {
            throw new System.NotImplementedException();
        }
    }
}