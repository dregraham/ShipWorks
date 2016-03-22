using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public static class ThreeDCartProductCache
    {
        private static readonly Dictionary<string, LruCache<int, ThreeDCartProduct>> StoreProductCaches =
           new Dictionary<string, LruCache<int, ThreeDCartProduct>>();

        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        public static LruCache<int, ThreeDCartProduct> GetStoreProductCache(string storeUrl, string accessToken)
        {
            // Create the key for the store
            string key = $"{storeUrl}-{accessToken}";

            LruCache<int, ThreeDCartProduct> productCache;

            // Try and get an existing cache based on the key.  If one is not found, create a new one and add it to the list of caches.
            if (!StoreProductCaches.TryGetValue(key, out productCache))
            {
                productCache = new LruCache<int, ThreeDCartProduct>(1000);
                StoreProductCaches.Add(key, productCache);
            }

            return productCache;
        }
    }
}