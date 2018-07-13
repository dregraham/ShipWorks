using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Service to manage Shopify locations
    /// </summary>
    [Component]
    public class ShopifyLocationService : IShopifyLocationService, IInitializeForCurrentDatabase
    {
        private readonly ConcurrentDictionary<long, long> primaryLocations = new ConcurrentDictionary<long, long>();

        /// <summary>
        /// Initialize the service
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) =>
            primaryLocations.Clear();

        /// <summary>
        /// Get the primary location id for a store
        /// </summary>
        public long GetPrimaryLocationID(IShopifyStoreEntity store, IShopifyWebClient webClient) =>
            primaryLocations.GetOrAdd(store.StoreID, x => webClient.GetShop().PrimaryLocationID.GetValueOrDefault(0));

        /// <summary>
        /// Get items grouped by the location id that should be used for them
        /// </summary>
        public IEnumerable<(long locationId, IEnumerable<IShopifyOrderItemEntity> items)> GetItemLocations(IShopifyWebClient webClient, IEnumerable<IShopifyOrderItemEntity> items)
        {
            throw new NotImplementedException();
        }
    }
}
