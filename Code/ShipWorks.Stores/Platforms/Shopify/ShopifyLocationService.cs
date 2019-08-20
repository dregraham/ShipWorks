﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Shopify.DTOs;

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
            primaryLocations.GetOrAdd(store.StoreID, _ => GetPrimaryLocationForStore(webClient));

        private static long GetPrimaryLocationForStore(IShopifyWebClient webClient)
        {
            var shopResult = webClient.GetShop();
            if (shopResult.Shop == null)
            {
                throw new ShopifyException("Could not get primary location for the Shopify store");
            }

            return shopResult.Shop.PrimaryLocationID;
        }

        /// <summary>
        /// Get the location ID set in the store. If none set, get shop default
        /// </summary>
        public long GetLocationID(IShopifyStoreEntity store, IShopifyWebClient webClient)
        {
            return store.ShopifyFulfillmentLocation == 0 ? GetPrimaryLocationID(store, webClient) : store.ShopifyFulfillmentLocation;
        }

        /// <summary>
        /// Get items grouped by the location id that should be used for them
        /// </summary>
        public IEnumerable<(long locationID, IEnumerable<IShopifyOrderItemEntity> items)> GetItemLocations(IShopifyWebClient webClient, long shopifyOrderID, IEnumerable<IShopifyOrderItemEntity> items)
        {
            var inventoryItems = items.Where(x => x.InventoryItemID.HasValue)
                .Select(x => (item: x, inventoryItemID: x.InventoryItemID.Value))
                .ToList();

            if (inventoryItems.None())
            {
                return Enumerable.Empty<(long, IEnumerable<IShopifyOrderItemEntity>)>();
            }

            var inventoryLevels = webClient.GetInventoryLevelsForItems(inventoryItems.Select(x => x.inventoryItemID)).InventoryLevels;
            if (inventoryLevels.None())
            {
                return Enumerable.Empty<(long, IEnumerable<IShopifyOrderItemEntity>)>();
            }

            var groupedLocations = inventoryLevels
                .GroupBy(inventoryLevel => inventoryLevel.LocationID)
                .Select(level => AssociateItemsWithLocation(level, inventoryItems))
                .OrderByDescending(x => x.items.Count())
                .ToList();

            return groupedLocations
                .Select((x, i) => (locationID: x.locationID, items: x.items.Except(groupedLocations.Take(i).SelectMany(y => y.items))))
                .Where(x => x.items.Any());
        }

        /// <summary>
        /// Associate items with the inventory level
        /// </summary>
        private static (long locationID, IEnumerable<IShopifyOrderItemEntity> items) AssociateItemsWithLocation(IGrouping<long, ShopifyInventoryLevel> level, List<(IShopifyOrderItemEntity item, long inventoryItemID)> inventoryItems) =>
            (
                locationID: level.Key,
                items: level.Select(x => inventoryItems.First(y => y.inventoryItemID == x.InventoryItemID).item)
            );
    }
}
