using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Manager of all the StoreTypes available in ShipWorks
    /// </summary>
    public class StoreTypeManagerWrapper : IStoreTypeManager
    {
        /// <summary>
        /// Returns all store types in ShipWorks
        /// </summary>
        public IEnumerable<StoreType> StoreTypes => StoreTypeManager.StoreTypes;

        /// <summary>
        /// Get the StoreType instance of the specified StoreEntity
        /// </summary>
        public StoreType GetType(StoreEntity store) => StoreTypeManager.GetType(store);

        /// <summary>
        /// The indexer of the class based on store type
        /// </summary>
        public StoreType GetType(StoreTypeCode typeCode) => StoreTypeManager.GetType(typeCode);

        /// <summary>
        /// Get the ShipmentType based on the given type code
        /// </summary>
        public StoreType GetType(StoreTypeCode typeCode, StoreEntity store) =>
            StoreTypeManager.GetType(typeCode, store);
    }
}