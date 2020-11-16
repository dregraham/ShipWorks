using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

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
        /// Get the store from Id
        /// </summary>
        IStoreEntity GetStoreReadOnly(long storeId);

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
        StoreEntity GetRelatedStore(IShipmentEntity shipment);

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

        /// <summary>
        /// Checks whether any stores have automatic validation enabled
        /// </summary>
        bool DoAnyStoresHaveAutomaticValidationEnabled();

        /// <summary>
        /// Get a collection of each store type in use by the current database. These are just a distinct list of the non-instanced types... with no stores attached.
        /// </summary>
        IEnumerable<StoreType> GetUniqueStoreTypes();

        /// <summary>
        /// Gets the number of setup stores in the database.
        /// </summary>
        int GetDatabaseStoreCount();

        /// <summary>
        /// Save the specified store, Translating known exceptions
        /// </summary>
        Task SaveStoreAsync(StoreEntity store);
    }
}