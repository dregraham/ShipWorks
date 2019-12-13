using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.Warehouse
{
    /// <summary>
    /// Rakuten warehouse item
    /// </summary>
    [Obfuscation]
    public class RakutenWarehouseItem
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
