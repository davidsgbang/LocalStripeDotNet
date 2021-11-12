using System.Net.Http;
using System.Threading.Tasks;

namespace LocalStripeDotNet.Server.Webhooks
{
    public interface IWebhookInitiator
    {
        Task<HttpResponseMessage> InitiateWebhook(StripeWebhookEvent webhookEvent);
    }
}