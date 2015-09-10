using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Cache mechanism for BigCommerce product images.  Since multiple BC stores can be downloading and product IDs are not unique across stores,
    /// each store needs it's own cache.  This class takes care of creating and getting store specific product image caches.
    /// </summary>
    public class BigCommerceProductImageCache
    {
        private static Dictionary<string, LruCache<int, BigCommerceProductImage>> storeProductImageCaches = new Dictionary<string, LruCache<int, BigCommerceProductImage>>();

        /// <summary>
        /// Gets the cache for specific store.
        /// </summary>
        /// <returns></returns>
        public static LruCache<int, BigCommerceProductImage> GetStoreProductImageCache(string apiUserName, string apiUrl, string apiToken)
        {
            // Create the key for the store
            string key = string.Format("{0}-{1}-{2}", apiUserName, apiUrl, apiToken);

            LruCache<int, BigCommerceProductImage> productImageCache;

            // Try and get an existing cache based on the key.  If one is not found, create a new one and add it to the list of caches.
            if (!storeProductImageCaches.TryGetValue(key, out productImageCache))
            {
                productImageCache = new LruCache<int, BigCommerceProductImage>(1000);
                storeProductImageCaches.Add(key, productImageCache);
            }

            return productImageCache;
        }
    }
}
