using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetOrderItemPrice
    {
        [JsonProperty("base_price")]
        public int BasePrice { get; set; }

        [JsonProperty("item_tax")]
        public double ItemTax { get; set; }

        [JsonProperty("item_shipping_cost")]
        public int ItemShippingCost { get; set; }

        [JsonProperty("item_shipping_tax")]
        public double ItemShippingTax { get; set; }
    }
}