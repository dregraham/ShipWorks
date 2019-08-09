using System;
using System.Collections.Generic;
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
        IDictionary<Guid, Store> GetStores();

        /// <summary>
        /// Get the given warehouseStoreIds OdbcStore
        /// </summary>
        OdbcStore GetStore(Guid warehouseStoreId);
    }
}