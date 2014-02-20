using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// A singleton for caching rates for a version of a shipment. To simplify using this
    /// cache for consumers, the interface gives the appearance that rates are cached solely
    /// by shipment, but the actual implementation takes into account the shipment type code
    /// as well. The result is that rate groups are cached individual shipping provider level.
    /// </summary>
    public class RateCache
    {
        private static readonly Lazy<RateCache> lazyInstance = new Lazy<RateCache>(() => new RateCache());
        private readonly LruCache<string, Dictionary<ShipmentTypeCode, RateGroup>> cachedRates;

        /// <summary>
        /// Prevents a default instance of the <see cref="RateCache"/> class from being created.
        /// </summary>
        private RateCache()
        {
            cachedRates = new LruCache<string, Dictionary<ShipmentTypeCode, RateGroup>>(10000);
        }

        /// <summary>
        /// Gets a handle to the RateCache singleton.
        /// </summary>
        public static RateCache Instance
        {
            get { return lazyInstance.Value; }
        }

        /// <summary>
        /// Adds an entry in the cache for the given key/value pair
        /// </summary>
        public void Add(ShipmentEntity key, RateGroup value)
        {
            string keyValue = GetCacheKey(key);

            if (!cachedRates.Contains(keyValue))
            {
                cachedRates[keyValue] = new Dictionary<ShipmentTypeCode, RateGroup>();
            }

            Dictionary<ShipmentTypeCode, RateGroup> rateMap = cachedRates[keyValue];
            rateMap[(ShipmentTypeCode)key.ShipmentType] = value;

            cachedRates[keyValue] = rateMap;
        }

        /// <summary>
        /// Gets the rate group for the given key.
        /// </summary>
        public RateGroup GetValue(ShipmentEntity key)
        {
            RateGroup rateGroup = null;

            if (this.Contains(key))
            {
                rateGroup = cachedRates[GetCacheKey(key)][(ShipmentTypeCode)key.ShipmentType];
            }

            return rateGroup;
        }

        /// <summary>
        /// Determines whether the cache [contains] [the specified key].
        /// </summary>
        public bool Contains(ShipmentEntity key)
        {
            bool containsItem = false;
            string keyValue = GetCacheKey(key);
            
            if (cachedRates.Contains(keyValue))
            {
                // The shipment is in the cache, but now we have to see if we have rates for 
                // the specific shipment type
                Dictionary<ShipmentTypeCode, RateGroup> rateMap = cachedRates[keyValue];
                if (rateMap.ContainsKey((ShipmentTypeCode)key.ShipmentType))
                {
                    containsItem = true;
                }
            }

            return containsItem;
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
