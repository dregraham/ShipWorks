using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Cache for storing Yahoo item weights, since we have to submit a whole new request
    /// solely for getting an items weight. So lets try an limit the number of calls we have to make.
    /// </summary>
    public class YahooProductWeightCache
    {
        private readonly Dictionary<string, LruCache<string, YahooCatalogItem>> storeProductWeightCaches;

        private static readonly Lazy<YahooProductWeightCache> instance =
            new Lazy<YahooProductWeightCache>(() => new YahooProductWeightCache());

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooProductWeightCache"/> class.
        /// </summary>
        public YahooProductWeightCache()
        {
            storeProductWeightCaches = new Dictionary<string, LruCache<string, YahooCatalogItem>>();
        }

        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        public LruCache<string, YahooCatalogItem> GetStoreProductWeightCache(string storeID)
        {
            // Create the key for the store
            string key = storeID;

            LruCache<string, YahooCatalogItem> productWeightCache;

            // Try and get an existing cache based on the key.  If one is not found, create a new one and add it to the list of caches.
            if (!storeProductWeightCaches.TryGetValue(key, out productWeightCache))
            {
                productWeightCache = new LruCache<string, YahooCatalogItem>(1000);
                storeProductWeightCaches.Add(key, productWeightCache);
            }

            return productWeightCache;
        }

        /// <summary>
        /// The instance value of the cache
        /// </summary>
        public static YahooProductWeightCache Instance => instance.Value;
    }
}