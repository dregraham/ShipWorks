using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Service to manage Shopify locations
    /// </summary>
    [Component]
    public class ShopifyLocationService : IShopifyLocationService
    {
        /// <summary>
        /// Get the primary location id for a store
        /// </summary>
        public long GetPrimaryLocationID(IShopifyWebClient webClient)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get items grouped by the location id that should be used for them
        /// </summary>
        public IEnumerable<(long locationId, IEnumerable<IShopifyOrderItemEntity> items)> GetItemLocations(IShopifyWebClient webClient, IEnumerable<IShopifyOrderItemEntity> items)
        {
            throw new NotImplementedException();
        }
    }
}
