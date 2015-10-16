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
    public interface IStoreTypeManager
    {
        /// <summary>
        /// Returns all store types in ShipWorks
        /// </summary>
        List<StoreType> StoreTypes { get; }

        /// <summary>
        /// Get the StoreType instance of the specified StoreEntity
        /// </summary>
        StoreType GetType(StoreEntity store);

        /// <summary>
        /// The indexer of the class based on store type
        /// </summary>
        StoreType GetType(StoreTypeCode typeCode);

        /// <summary>
        /// Get the ShipmentType based on the given type code
        /// </summary>
        StoreType GetType(StoreTypeCode typeCode, StoreEntity store);
    }
}
