﻿using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Service to manage Shopify locations
    /// </summary>
    public interface IShopifyLocationService
    {
        /// <summary>
        /// Get the primary location id for a store
        /// </summary>
        long GetPrimaryLocationID(IShopifyStoreEntity store, IShopifyWebClient webClient);

        /// <summary>
        /// Get the location ID set in the store. If none set, get shop default
        /// </summary>
        long GetLocationID(IShopifyStoreEntity store, IShopifyWebClient webClient);

        /// <summary>
        /// Get items grouped by the location id that should be used for them
        /// </summary>
        IEnumerable<(long locationID, IEnumerable<IShopifyOrderItemEntity> items)> GetItemLocations(IShopifyWebClient webClient, long shopifyOrderID, IEnumerable<IShopifyOrderItemEntity> items);
    }
}
