using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Algorithms and functions for working with stores.
    /// </summary>
    public interface IStoreManager
    {
        /// <summary>
        /// Get the store for the related entity
        /// </summary>
        StoreEntity GetRelatedStore(long entityID);

        /// <summary>
        /// Get the store from Id
        /// </summary>
        StoreEntity GetStore(long storeId);

        /// <summary>
        /// Get the current list of stores.  All stores are returned, regardless of security.
        /// </summary>
        IEnumerable<StoreEntity> GetAllStores();

        /// <summary>
        /// Get all stores, regardless of security, that are currently enabled for downloading and shipping
        /// </summary>
        IEnumerable<StoreEntity> GetEnabledStores();

        /// <summary>
        /// Get the store for the related Shipment
        /// </summary>
        StoreEntity GetRelatedStore(ShipmentEntity shipment);

        /// <summary>
        /// Saves the store.
        /// </summary>
        void SaveStore(StoreEntity store);

        /// <summary>
        /// Creates the online status filters for the given store.
        /// </summary>
        void CreateStoreStatusFilters(IWin32Window owner, StoreEntity store);

        /// <summary>
        /// Notify the underlying data manager that there may have been changes
        /// </summary>
        void CheckForChanges();
    }
}