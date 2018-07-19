using System.Collections.Concurrent;
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
            primaryLocations.GetOrAdd(store.StoreID, x => webClient.GetShop().Shop.PrimaryLocationID);

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

            var groupedLocations = webClient
                .GetInventoryLevelsForItems(inventoryItems.Select(x => x.inventoryItemID))
                .InventoryLevels
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
