using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Algorithms and functions for working with stores.
    /// </summary>
    /// <remarks>This is an instance that wraps the static StoreManager until we can replace that class</remarks>
    public class StoreManagerWrapper : IStoreManager
    {
        /// <summary>
        /// Get the current list of stores.  All stores are returned, regardless of security.
        /// </summary>
        public IEnumerable<StoreEntity> GetAllStores()
        {
            return StoreManager.GetAllStores();
        }

        /// <summary>
        /// Get all stores, regardless of security, that are currently enabled for downloading and shipping
        /// </summary>
        public IEnumerable<StoreEntity> GetEnabledStores()
        {
            return StoreManager.GetEnabledStores();
        }

        /// <summary>
        /// Get the store with the given ID.  If it does not exist, null is returned
        /// </summary>
        public StoreEntity GetStore(long storeID)
        {
            return StoreManager.GetStore(storeID);
        }
    }
}