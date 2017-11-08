using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class Item
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("qty_ordered")]
        public double QtyOrdered { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("parent_item_id")]
        public int? ParentItemId { get; set; }

        [JsonProperty("parent_item")]
        public ParentItem ParentItem { get; set; }

        [JsonProperty("product_option")]
        public ProductOption ProductOption { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }
    }
}