using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetOrderItemPrice
    {
        [JsonProperty("base_price")]
        public decimal BasePrice { get; set; }

        [JsonProperty("item_tax")]
        public decimal ItemTax { get; set; }

        [JsonProperty("item_shipping_cost")]
        public decimal ItemShippingCost { get; set; }

        [JsonProperty("item_shipping_tax")]
        public decimal ItemShippingTax { get; set; }
    }
}