using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class Order
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonProperty("discount_description")]
        public string DiscountDescription { get; set; }

        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("increment_id")]
        public int IncrementId { get; set; }
        
        [JsonProperty("grand_total")]
        public double GrandTotal { get; set; }

        [JsonProperty("shipping_amount")]
        public double ShippingAmount { get; set; }

        [JsonProperty("shipping_description")]
        public string ShippingDescription { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("tax_amount")]
        public double TaxAmount { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("items")]
        public IEnumerable<Item> Items { get; set; }

        [JsonProperty("status_histories")]
        public IEnumerable<StatusHistory> StatusHistories { get; set; }

        [JsonProperty("billing_address")]
        public BillingAddress BillingAddress { get; set; }

        [JsonProperty("payment")]
        public Payment Payment { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}