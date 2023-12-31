using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Interface for StoreTypeManager
    /// </summary>
    public interface IStoreTypeManager
    {
        /// <summary>
        /// Returns all store types in ShipWorks
        /// </summary>
        IEnumerable<StoreType> StoreTypes { get; }

        /// <summary>
        /// Get the StoreType instance of the specified StoreEntity
        /// </summary>
        StoreType GetType(StoreEntity store);

        /// <summary>
        /// Get the StoreType instance of the specified StoreID
        /// </summary>
        StoreType GetType(long storeID);

        /// <summary>
        /// The indexer of the class based on store type
        /// </summary>
        StoreType GetType(StoreTypeCode typeCode);

        /// <summary>
        /// Get the ShipmentType based on the given type code
        /// </summary>
        StoreType GetType(StoreTypeCode typeCode, StoreEntity store);

        /// <summary>
        /// Get the store type from the shipment
        /// </summary>
        StoreType GetType(ShipmentEntity shipment);

        /// <summary>
        /// Check to see if the given StoreTypeCode is GenericModule based
        /// </summary>
        bool IsStoreTypeCodeGenericModuleBased(StoreTypeCode storeTypeCode);
    }
}