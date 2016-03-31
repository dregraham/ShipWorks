using Newtonsoft.Json;
using System;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class Shipment
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("order_id")]
        public long? OrderId { get; set; }

        [JsonProperty("shipped_at")]
        public DateTime? ShippedAt { get; set; }

        [JsonProperty("tracking_numbers")]
        public string TrackingNumbers { get; set; }

        [JsonProperty("tracking_url")]
        public string TrackingUrl { get; set; }

        [JsonProperty("shipping_method")]
        public string ShippingMethod { get; set; }

        [JsonProperty("shipping_method_id")]
        public int? ShippingMethodId { get; set; }

        [JsonProperty("number_of_packages")]
        public object NumberOfPackages { get; set; }

        [JsonProperty("total_weight")]
        public object TotalWeight { get; set; }

        [JsonProperty("provider_base_shipping_cost")]
        public string ProviderBaseShippingCost { get; set; }

        [JsonProperty("provider_insurance_cost")]
        public object ProviderInsuranceCost { get; set; }

        [JsonProperty("provider_handling_cost")]
        public object ProviderHandlingCost { get; set; }

        [JsonProperty("provider_total_shipping_cost")]
        public object ProviderTotalShippingCost { get; set; }

        [JsonProperty("email_sent")]
        public bool? EmailSent { get; set; }

        [JsonProperty("private_comment")]
        public string PrivateComment { get; set; }

        [JsonProperty("shipping_comment")]
        public string ShippingComment { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("shipping_method_type")]
        public string ShippingMethodType { get; set; }

        [JsonProperty("shipment_name")]
        public string ShipmentName { get; set; }
    }
}
