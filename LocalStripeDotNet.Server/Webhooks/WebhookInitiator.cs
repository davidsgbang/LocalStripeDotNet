using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using LocalStripeDotNet.Server.Generators;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Stripe;

namespace LocalStripeDotNet.Server.Webhooks
{
    public class WebhookInitiator : IWebhookInitiator
    {
        private readonly HttpClient targetClient;
        private readonly Uri targetUri;
        private readonly Randomizer randomizer;

        public WebhookInitiator(string webhookTarget)
        {
            this.targetClient = new HttpClient();
            this.targetUri = new Uri(webhookTarget);
            this.randomizer = new Randomizer();

            Console.WriteLine($"Sending Webhook to {this.targetUri}");
        }

        public async Task<HttpResponseMessage> InitiateWebhook(StripeWebhookEvent webhookEvent)
        {
            string eventId = StripeIdPrefixes.GetPrefix(nameof(Event)) + this.randomizer.AlphaNumeric(16);
            webhookEvent.Id = eventId;
            
            Console.WriteLine($"Sending {webhookEvent.Type}:{eventId} to {this.targetUri}");

            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(
                webhookEvent, 
                new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy(),
                    },
                    Formatting = Formatting.Indented
                });
            
            var message = await this.targetClient.PostAsync(
                this.targetUri, 
                new StringContent(stringPayload, Encoding.UTF8, "application/json"));
            return message;
        }
    }
}