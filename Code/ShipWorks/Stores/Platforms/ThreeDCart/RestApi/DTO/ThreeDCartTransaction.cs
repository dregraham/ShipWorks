using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ThreeDCartTransaction
    {
        [JsonProperty("TransactionIndexID")]
        public int TransactionIndexID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("TransactionID")]
        public string TransactionID { get; set; }

        [JsonProperty("TransactionDateTime")]
        public DateTime TransactionDateTime { get; set; }

        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("TransactionMethod")]
        public string TransactionMethod { get; set; }

        [JsonProperty("TransactionAmount")]
        public double TransactionAmount { get; set; }

        [JsonProperty("TransactionApproval")]
        public string TransactionApproval { get; set; }

        [JsonProperty("TransactionReference")]
        public string TransactionReference { get; set; }

        [JsonProperty("TransactionGatewayID")]
        public int TransactionGatewayID { get; set; }

        [JsonProperty("TransactionCVV2")]
        public string TransactionCVV2 { get; set; }

        [JsonProperty("TransactionAVS")]
        public string TransactionAVS { get; set; }

        [JsonProperty("TransactionResponseText")]
        public string TransactionResponseText { get; set; }

        [JsonProperty("TransactionResponseCode")]
        public string TransactionResponseCode { get; set; }

        [JsonProperty("TransactionCaptured")]
        public int TransactionCaptured { get; set; }
    }
}