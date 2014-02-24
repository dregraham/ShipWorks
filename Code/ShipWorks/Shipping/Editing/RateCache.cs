using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

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

        /// <summary>
        /// Save an entry in the cache for the given key/value pair. If an entry already
        /// exists, meaning an entry for both the shipment entity and the shipment type 
        /// code is present, it is overwritten.
        /// </summary>
        public void Save(string key, RateGroup value)
        {
            cachedRates[key] = value;
        }

        /// <summary>
        /// Gets the rate group for the given key.
        /// </summary>
        public RateGroup GetRateGroup(string key)
        {
            return cachedRates[key];
        }

        /// <summary>
        /// Gets a value indicating whether the cache is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is empty]; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get { return cachedRates.Keys.Count == 0; }
        }

        /// <summary>
        /// Clears all entries in the cache.
        /// </summary>
        public void Clear()
        {
            cachedRates.Clear();
        }

        /// <summary>
        /// Clears rates for the given key.
        /// </summary>
        public void Remove(string key)
        {
            cachedRates.Remove(key);
        }

        /// <summary>
        /// Determines whether the cache [contains] [the specified key].
        /// </summary>
        public bool Contains(string key)
        {
            return cachedRates.Contains(key);
        }
        
        /// <summary>
        /// Invalids the rates for the given shipment.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public void InvalidateRates(string key)
        {
            if (cachedRates.Contains(key))
            {
                cachedRates[key].OutOfDate = true;
            }
        }
    }
}
