using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Algorithms and functions for working with stores.
    /// </summary>
    public interface IStoreManager
    {
        /// <summary>
        /// Get the current list of stores.  All stores are returned, regardless of security.
        /// </summary>
        IEnumerable<StoreEntity> GetAllStores();

        /// <summary>
        /// Get all stores, regardless of security, that are currently enabled for downloading and shipping
        /// </summary>
        IEnumerable<StoreEntity> GetEnabledStores();
    }
}