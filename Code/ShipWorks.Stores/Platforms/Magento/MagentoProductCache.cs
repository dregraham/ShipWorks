using System.Collections.Concurrent;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Caching for Magento Products
    /// </summary>
    public class MagentoProductCache : IMagentoProductCache, IInitializeForCurrentDatabase
    {
        private readonly ConcurrentDictionary<long, LruCache<string, Product>> storeProductCaches =
            new ConcurrentDictionary<long, LruCache<string, Product>>();

        /// <summary>
        /// Clear the cache when connecting to a new database
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) => storeProductCaches.Clear();

        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        public LruCache<string, Product> GetStoreProductCache(long storeID)
            => storeProductCaches.GetOrAdd(storeID, new LruCache<string, Product>(1000));
    }
}