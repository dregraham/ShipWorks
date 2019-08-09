using System;
using System.Collections.Generic;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

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
        /// Get the given warehouseStoreIds import map
        /// </summary>
        IOdbcFieldMap GetImportMap(Guid warehouseStoreId);

        /// <summary>
        /// Get the given warehouseStoreIds upload map
        /// </summary>
        IOdbcFieldMap GetUploadMap(Guid warehouseStoreId);
    }
}