using System;
using Newtonsoft.Json;
using Stripe.Infrastructure;

namespace LocalStripeDotNet.Server.Webhooks
{
    public struct StripeWebhookEvent
    {
        public string Id { get; set; }
        
        public StripeWebhookEventData Data { get; set; }
        
        public StripeWebhookEventRequest Request { get; set; }
        
        public string Type { get; set; }
        
        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }
        
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Created { get; set; }
        
        public bool Livemode { get; set; }
    }

    public struct StripeWebhookEventData
    {
        public dynamic? Object { get; set; }
        
        [JsonProperty("previous_attributes")]
        public dynamic? PreviousAttributes { get; set; }
        
        [JsonProperty("raw_object")]
        public dynamic? RawObject { get; set; }
    }
    
    public struct StripeWebhookEventRequest
    {
        public string Id { get; set; }
        public string IdempotencyKey { get; set; }
    }
}