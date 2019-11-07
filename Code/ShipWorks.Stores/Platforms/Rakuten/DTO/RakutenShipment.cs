using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Shipping entity returned by Rakuten
    /// </summary>
    public class RakutenShipment
    {
        [JsonProperty("orderPackageId")]
        public string OrderPackageID { get; set; }

        [JsonProperty("shippingMethod")]
        public string ShippingMethod { get; set; }

        [JsonProperty("shippingStatus")]
        public string ShippingStatus { get; set; }

        [JsonProperty("shippingFee")]
        public decimal ShippingFee { get; set; }
        
    }
}