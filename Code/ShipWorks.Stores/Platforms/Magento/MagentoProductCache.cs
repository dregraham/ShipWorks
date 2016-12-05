using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne;

namespace ShipWorks.Stores.Platforms.Magento
{
    public class MagentoProductCache
    {
        private readonly Dictionary<string, LruCache<string, Product>> storeProductCaches;

        private static readonly Lazy<MagentoProductCache> instance =
            new Lazy<MagentoProductCache>(() => new MagentoProductCache());

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoProductCache"/> class.
        /// </summary>
        public MagentoProductCache()
        {
            storeProductCaches = new Dictionary<string, LruCache<string, Product>>();
        }

        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        /// <returns></returns>
        public LruCache<string, Product> GetStoreProductCache(long storeID)
        {
            // Create the key for the store
            string key = $"{storeID}";

            LruCache<string, Product> productCache;

            // Try and get an existing cache based on the key.  If one is not found, create a new one and add it to the list of caches.
            if (!storeProductCaches.TryGetValue(key, out productCache))
            {
                productCache = new LruCache<string, Product>(1000);
                storeProductCaches.Add(key, productCache);
            }

            return productCache;
        }

        public static MagentoProductCache Instance => instance.Value;
    }
}