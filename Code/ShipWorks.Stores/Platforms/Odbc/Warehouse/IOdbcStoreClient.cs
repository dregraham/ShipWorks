using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.Warehouse
{
    /// <summary>
    /// Client for interacting with odbc stores and warehouse
    /// </summary>
    public interface IOdbcStoreClient
    {
        /// <summary>
        /// Get the warehouse customers odbc stores where key is the warehouse store id
        /// </summary>
        Task<Dictionary<Guid, Store>> GetStores();

        /// <summary>
        /// Get the given warehouseStoreIds OdbcStore
        /// </summary>
        Task<OdbcStore> GetStore(Guid warehouseStoreId);
    }
}