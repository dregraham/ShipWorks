using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Response for the locations request
    /// </summary>
    [Obfuscation]
    public class ShopifyLocationsResponse
    {
        /// <summary>
        /// Available locations
        /// </summary>
        [JsonProperty("locations")]
        public IEnumerable<ShopifyLocation> Locations { get; set; }
    }
}
