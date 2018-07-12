using Interapptive.Shared.Net;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Inventory level for a Shopify product
    /// </summary>
    public class ShopifyInventoryLevel
    {
        private readonly JToken shopifyInventoryLevel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyInventoryLevel(JToken shopifyInventoryLevel)
        {
            this.shopifyInventoryLevel = shopifyInventoryLevel;
        }

        /// <summary>
        /// The Available
        /// </summary>
        public long? Available => shopifyInventoryLevel.GetValue<long?>("available");

        /// <summary>
        /// The LocationID
        /// </summary>
        public long? LocationID => shopifyInventoryLevel.GetValue<long?>("location_id");

        /// <summary>
        /// The InventoryItemID
        /// </summary>
        public long? InventoryItemID => shopifyInventoryLevel.GetValue<long?>("inventory_item_id");
    }
}
