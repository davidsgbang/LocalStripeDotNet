using System;
using System.Collections.Generic;
using Stripe.Issuing;

namespace LocalStripeDotNet.Server.Repositories.InMemory
{
    public class InMemoryIssuingCardholderRepository : IStripeRepository<Cardholder>
    {
        private readonly Dictionary<string, Cardholder> repository;

        public InMemoryIssuingCardholderRepository()
        {
            this.repository = new Dictionary<string, Cardholder>();
        }
        
        public bool TryGet(string id, out Cardholder record)
        {
            return this.repository.TryGetValue(id, out record);
        }

        public void Insert(Cardholder record)
        {
            if (!this.repository.TryAdd(record.Id, record))
            {
                throw new Exception($"Cannot add IssuingCardholder {record.Id}");
            }
        }

        public void Delete(string id)
        {
            if (!this.repository.ContainsKey(id))
            {
                throw new ArgumentException($"IssuingCard {id} cannot be updated, since it does not exist");
            }

            this.repository.Remove(id);
        }

        public Cardholder Update(Cardholder record)
        {
            if (!this.repository.ContainsKey(record.Id))
            {
                throw new ArgumentException($"IssuingCard {record.Id} cannot be updated, since it does not exist");
            }
            
            this.repository[record.Id] = record;

            return this.repository[record.Id];
        }

        public void Flush()
        {
            this.repository.Clear();
        }
    }
}