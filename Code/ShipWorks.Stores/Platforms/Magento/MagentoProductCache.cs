using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Caching for Magento Products
    /// </summary>
    public class MagentoProductCache
    {
        private readonly Dictionary<long, LruCache<string, Product>> storeProductCaches;

        private static readonly Lazy<MagentoProductCache> instance =
            new Lazy<MagentoProductCache>(() => new MagentoProductCache());

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoProductCache"/> class.
        /// </summary>
        private MagentoProductCache()
        {
            storeProductCaches = new Dictionary<long, LruCache<string, Product>>();
        }

        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        /// <returns></returns>
        public LruCache<string, Product> GetStoreProductCache(long storeID)
        {
            LruCache<string, Product> productCache;

            // Try and get an existing cache based on the key.  If one is not found, create a new one and add it to the list of caches.
            if (!storeProductCaches.TryGetValue(storeID, out productCache))
            {
                productCache = new LruCache<string, Product>(1000);
                storeProductCaches.Add(storeID, productCache);
            }

            return productCache;
        }

        public static MagentoProductCache Instance => instance.Value;
    }
}