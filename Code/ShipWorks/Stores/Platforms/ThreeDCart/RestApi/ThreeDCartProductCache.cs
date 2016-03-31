using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public class ThreeDCartProductCache
    {
        private readonly Dictionary<string, LruCache<int, ThreeDCartProduct>> storeProductCaches =
           new Dictionary<string, LruCache<int, ThreeDCartProduct>>();

        private static readonly Lazy<ThreeDCartProductCache> instance =
            new Lazy<ThreeDCartProductCache>(() => new ThreeDCartProductCache());

        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        public LruCache<int, ThreeDCartProduct> GetStoreProductCache(string storeUrl, string accessToken)
        {
            // Create the key for the store
            string key = $"{storeUrl}-{accessToken}";

            LruCache<int, ThreeDCartProduct> productCache;

            // Try and get an existing cache based on the key.  If one is not found, create a new one and add it to the list of caches.
            if (!storeProductCaches.TryGetValue(key, out productCache))
            {
                productCache = new LruCache<int, ThreeDCartProduct>(1000);
                storeProductCaches.Add(key, productCache);
            }

            return productCache;
        }

        /// <summary>
        /// The instance value of the cache
        /// </summary>
        public static ThreeDCartProductCache Instance => instance.Value;
    }
}