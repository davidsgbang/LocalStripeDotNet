using Stripe;

namespace LocalStripeDotNet.Server.Repositories
{
    public interface IStripeRepository<T>
    {
        bool TryGet(string id, out T record);
        void Insert(T record);
        void Delete(string id);
        T Update(T record);
        void Flush();
    }
}