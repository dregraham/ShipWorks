using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Response from Shopify for Inventory Levels
    /// </summary>
    [Obfuscation]
    public class ShopifyInventoryLevelsResponse
    {
        /// <summary>
        /// Available locations
        /// </summary>
        [JsonProperty("inventory_levels")]
        public IEnumerable<ShopifyInventoryLevel> InventoryLevels { get; set; }
    }
}
