using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Stripe;

namespace LocalStripeDotNet.Server.Webhooks
{
    public struct StripeWebhookEvent
    {
        public string Id { get; set; }
        
        public StripeWebhookEventData Data { get; set; }
        
        public StripeWebhookEventRequest Request { get; set; }
        
        public string Type { get; set; }
        
        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; }
        
        public DateTime Created { get; set; }
        
        public bool Livemode { get; set; }
    }

    public struct StripeWebhookEventData
    {
        public dynamic? Object { get; set; }
        
        [JsonPropertyName("previous_attributes")]
        public dynamic? PreviousAttributes { get; set; }
        
        [JsonPropertyName("raw_object")]
        public dynamic? RawObject { get; set; }
    }
    
    public struct StripeWebhookEventRequest
    {
        public string Id { get; set; }
        public string IdempotencyKey { get; set; }
    }
}