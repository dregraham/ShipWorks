using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Order Item Bundle Components entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorOrderItemBundleComponents
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("FulfillmentID")]
        public int FulfillmentID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("OrderItemID")]
        public int OrderItemID { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("ProductID")]
        public int ProductID { get; set; }

        [JsonProperty("BundleProductID")]
        public int BundleProductID { get; set; }

        [JsonProperty("Sku")]
        public string Sku { get; set; }

        [JsonProperty("BundleSku")]
        public string BundleSku { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }
    }
}