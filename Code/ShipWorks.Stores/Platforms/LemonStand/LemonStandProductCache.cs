using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.LemonStand.DTO;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    public static class LemonStandProductCache
    {
        private static readonly Dictionary<string, LruCache<int, LemonStandItem>> storeProductCaches =
            new Dictionary<string, LruCache<int, LemonStandItem>>();

        /// <summary>
        ///     Gets the cache for specific store.
        /// </summary>
        /// <returns></returns>
        public static LruCache<int, LemonStandItem> GetStoreProductImageCache(string storeUrl, string accessToken)
        {
            // Create the key for the store
            string key = storeUrl + "-" + accessToken;

            LruCache<int, LemonStandItem> productCache;

            // Try and get an existing cache based on the key.  If one is not found, create a new one and add it to the list of caches.
            if (!storeProductCaches.TryGetValue(key, out productCache))
            {
                productCache = new LruCache<int, LemonStandItem>(1000);
                storeProductCaches.Add(key, productCache);
            }

            return productCache;
        }
    }
}