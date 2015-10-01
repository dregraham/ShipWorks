using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class LemonStandInvoice
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("shipments")]
        public LemonStandShipment Shipments { get; set; }

        [JsonProperty("total_shipping_quote")]
        public string TotalShippingQuote { get; set; }

        [JsonProperty("tax_total")]
        public string TaxTotal { get; set; }

        [JsonProperty("total_discount")]
        public string TotalDiscount { get; set; }

        [JsonProperty("subtotal")]
        public string Subtotal { get; set; }
    }
}