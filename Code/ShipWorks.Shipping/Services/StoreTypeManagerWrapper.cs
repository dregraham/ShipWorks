using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Manager of all the StoreTypes available in ShipWorks
    /// </summary>
    public class StoreTypeManagerWrapper : IStoreTypeManager
    {
        /// <summary>
        /// Returns all store types in ShipWorks
        /// </summary>
        public List<StoreType> StoreTypes
        {
            get { return StoreTypeManager.StoreTypes; }
        }

        /// <summary>
        /// Get the StoreType instance of the specified StoreEntity
        /// </summary>
        public StoreType GetType(StoreEntity store)
        {
            return StoreTypeManager.GetType(store);
        }

        /// <summary>
        /// The indexer of the class based on store type
        /// </summary>
        public StoreType GetType(StoreTypeCode typeCode)
        {
            return StoreTypeManager.GetType(typeCode);
        }

        /// <summary>
        /// Get the ShipmentType based on the given type code
        /// </summary>
        public StoreType GetType(StoreTypeCode typeCode, StoreEntity store)
        {
            return StoreTypeManager.GetType(typeCode, store);
        }
    }
}
