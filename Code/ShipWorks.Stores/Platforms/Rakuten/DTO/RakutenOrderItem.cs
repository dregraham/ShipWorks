using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Order Item entity returned by Rakuten
    /// </summary>
    public class RakutenOrderItem
    {
        [JsonProperty("baseSku")]
        public string BaseSku { get; set; }

        [JsonProperty("orderItemId")]
        public string OrderItemID { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        [JsonProperty("itemTotal")]
        public decimal ItemTotal { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }
    }
}