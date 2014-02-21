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

        ///// <summary>
        ///// Save an entry in the cache for the given key/value pair. If an entry already
        ///// exists, meaning an entry for both the shipment entity and the shipment type 
        ///// code is present, it is overwritten.
        ///// </summary>
        //public void Save(ShipmentEntity key, RateGroup value)
        //{
        //    string keyValue = GetCacheKey(key);
        //    cachedRates[keyValue] = value;
        //}

        /// <summary>
        /// Save an entry in the cache for the given key/value pair. If an entry already
        /// exists, meaning an entry for both the shipment entity and the shipment type 
        /// code is present, it is overwritten.
        /// </summary>
        public void Save(string key, RateGroup value)
        {
            cachedRates[key] = value;
        }

        ///// <summary>
        ///// Gets the rate group for the given key.
        ///// </summary>
        //public RateGroup GetValue(ShipmentEntity key)
        //{
        //    return cachedRates[GetCacheKey(key)];
        //}

        /// <summary>
        /// Gets the rate group for the given key.
        /// </summary>
        public RateGroup GetValue(string key)
        {
            return cachedRates[key];
        }
        
        ///// <summary>
        ///// Clears rates for the given key.
        ///// </summary>
        //public void Clear(ShipmentEntity key)
        //{
        //    cachedRates.Remove(GetCacheKey(key));
        //}

        /// <summary>
        /// Clears rates for the given key.
        /// </summary>
        public void Clear(string key)
        {
            cachedRates.Remove(key);
        }

        ///// <summary>
        ///// Determines whether the cache [contains] [the specified key].
        ///// </summary>
        //public bool Contains(ShipmentEntity key)
        //{
        //    return cachedRates.Contains(GetCacheKey(key));
        //}

        /// <summary>
        /// Determines whether the cache [contains] [the specified key].
        /// </summary>
        public bool Contains(string key)
        {
            return cachedRates.Contains(key);
        }

        ///// <summary>
        ///// Invalids the rates for the given shipment.
        ///// </summary>
        ///// <param name="key">The key.</param>
        ///// <returns></returns>
        //public void InvalidateRates(ShipmentEntity key)
        //{
        //    string keyValue = GetCacheKey(key);

        //    if (cachedRates.Contains(keyValue))
        //    {
        //        cachedRates[keyValue].OutOfDate = true;
        //    }
        //}

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

        /// <summary>
        /// A helper method to build the key used in the cache. The key is the 
        /// string representation of the RowVersion property, so the rates are
        /// only re-fetched when there is an actual change to the shipment.
        /// </summary>
        private static string GetCacheKey(ShipmentEntity shipment)
        {
            //ShipmentEntity filledShipment = ShippingManager.GetShipment(shipment.ShipmentID);
            return ShipmentTypeManager.GetType(shipment).GetRatingHash(shipment);


            //StringBuilder valueToBeHashed = new StringBuilder();

            //foreach (IEntityField2 field in filledShipment.Fields)
            //{
            //    if (field.Name != ShipmentFields.RowVersion.Name)
            //    {
            //        valueToBeHashed.Append(field.CurrentValue);
            //    }
            //}

            //List<IEntity2> shipmentGraph = new ObjectGraphUtils().ProduceTopologyOrderedList(filledShipment);
            //foreach (IEntity2 entity in shipmentGraph)
            //{
            //    foreach (IEntityField2 field in entity.Fields)
            //    {
            //        valueToBeHashed.Append(field.CurrentValue);
            //    }
            //}





            // Use the string value of the row version; using the byte[] was not
            // being indexed/looked up correctly in the cache
            //return BitConverter.ToString(shipment.RowVersion);
        }
    }
}
