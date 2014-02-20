using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// A singleton for caching rates for a version of a shipment.
    /// </summary>
    public class RateCache
    {
        private static readonly Lazy<RateCache> lazyInstance = new Lazy<RateCache>(() => new RateCache());
        private readonly LruCache<string, RateGroup> cachedRates;

        /// <summary>
        /// Prevents a default instance of the <see cref="RateCache"/> class from being created.
        /// </summary>
        private RateCache()
        {
            cachedRates = new LruCache<string, RateGroup>(10000);
        }

        /// <summary>
        /// Gets a handle to the RateCache singleton.
        /// </summary>
        public static RateCache Instance
        {
            get { return lazyInstance.Value; }
        }

        public void Add(ShipmentEntity key, RateGroup value)
        {
            cachedRates[GetCacheKey(key)] = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="RateGroup"/> with the specified key.
        /// </summary>
        public RateGroup this[ShipmentEntity key]
        {
            get { return cachedRates[GetCacheKey(key)]; }
            set { cachedRates[GetCacheKey(key)] = value; }
        }

        /// <summary>
        /// Determines whether the cache [contains] [the specified key].
        /// </summary>
        public bool Contains(ShipmentEntity key)
        {
            return cachedRates.Contains(GetCacheKey(key));
        }

        /// <summary>
        /// A helper method to build the key used in the cache. The key is the 
        /// string representation of the RowVersion property, so the rates are
        /// only re-fetched when there is an actual change to the shipment.
        /// </summary>
        private static string GetCacheKey(ShipmentEntity shipment)
        {
            // Use the string value of the row version; using the byte[] was not
            // being indexed/looked up correctly in the cache
            return BitConverter.ToString(shipment.RowVersion);
        }
    }
}
