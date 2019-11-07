using System;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Order entity returned by Rakuten
    /// </summary>
    public class RakutenOrder
    {
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty("orderStatus")]
        public string OrderStatus { get; set; }

        [JsonProperty("orderTotal")]
        public decimal OrderTotal { get; set; }

        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }
    }
}