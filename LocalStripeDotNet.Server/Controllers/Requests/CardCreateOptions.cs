using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LocalStripeDotNet.Server.Controllers.Requests
{
    public class CardCreateOptions {
        public CardCreateOptions() {
            this.Cardholder = string.Empty;
            this.Currency = string.Empty;
            this.Type = string.Empty;
        }

        public CardCreateOptions(string cardholder, string currency, string type)
        {
            this.Cardholder = cardholder;
            this.Currency = currency;
            this.Type = type;
        }

        public string Cardholder { get; set; }

        public string Currency { get; set; }

        public string Type { get; set; }

        public Dictionary<string,string>? Metadata { get; set; }

        public IssuingCardStatus? Status { get; set; }


        [JsonPropertyName("replacement_for")]
        public string? ReplacementFor { get; set; }

        [JsonPropertyName("replacement_reason")]
        public string? ReplacementReason { get; set; }
    }

    public enum IssuingCardStatus {
        Active,
        Inactive
    }
}