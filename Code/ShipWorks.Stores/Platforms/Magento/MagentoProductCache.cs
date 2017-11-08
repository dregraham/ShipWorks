using System.Collections.Concurrent;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Caching for Magento Products
    /// </summary>
    [Component]
    public class MagentoProductCache : IMagentoProductCache, IInitializeForCurrentDatabase
    {
        private readonly ConcurrentDictionary<long, LruCache<string, Product>> storeProductSkuCaches =
            new ConcurrentDictionary<long, LruCache<string, Product>>();

        private readonly ConcurrentDictionary<long, LruCache<int, Product>> storeProductIdCaches =
            new ConcurrentDictionary<long, LruCache<int, Product>>();

        /// <summary>
        /// Clear the cache when connecting to a new database
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode)
        {
            storeProductSkuCaches.Clear();
            storeProductIdCaches.Clear();
        }

        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        public LruCache<string, Product> GetStoreProductBySkuCache(long storeID)
            => storeProductSkuCaches.GetOrAdd(storeID, new LruCache<string, Product>(1000));
        
        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        public LruCache<int, Product> GetStoreProductByIdCache(long storeID)
            => storeProductIdCaches.GetOrAdd(storeID, new LruCache<int, Product>(1000));
    }
}