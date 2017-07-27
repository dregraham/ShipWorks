using System.Collections.Generic;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Manager of all the StoreTypes available in ShipWorks
    /// </summary>
    public class StoreTypeManagerWrapper : IStoreTypeManager
    {
        private readonly IStoreManager storeManager;
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreTypeManagerWrapper(IStoreManager storeManager, ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Returns all store types in ShipWorks
        /// </summary>
        public IEnumerable<StoreType> StoreTypes => StoreTypeManager.StoreTypes;

        /// <summary>
        /// Get the StoreType instance of the specified StoreEntity
        /// </summary>
        public StoreType GetType(StoreEntity store) => StoreTypeManager.GetType(store.StoreTypeCode, store, lifetimeScope);

        /// <summary>
        /// Get the StoreType instance of the specified StoreID
        /// </summary>
        public StoreType GetType(long storeID)
        {
            StoreEntity store = storeManager.GetStore(storeID);
            return StoreTypeManager.GetType(store.StoreTypeCode, store, lifetimeScope);
        }

        /// <summary>
        /// The indexer of the class based on store type
        /// </summary>
        public StoreType GetType(StoreTypeCode typeCode) => StoreTypeManager.GetType(typeCode, null, lifetimeScope);

        /// <summary>
        /// Get the StoreType based on the given type code
        /// </summary>
        public StoreType GetType(StoreTypeCode typeCode, StoreEntity store) =>
            StoreTypeManager.GetType(typeCode, store, lifetimeScope);

        /// <summary>
        /// Get the store type from the shipment
        /// </summary>
        public StoreType GetType(ShipmentEntity shipment)
        {
            StoreEntity store = storeManager.GetRelatedStore(shipment.ShipmentID);
            return StoreTypeManager.GetType(store.StoreTypeCode, store, lifetimeScope);
        }
    }
}