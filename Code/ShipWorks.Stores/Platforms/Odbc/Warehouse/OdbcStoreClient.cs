using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Warehouse
{
    /// <summary>
    /// Client for interacting with Odbc Stores and the hub
    /// </summary>
    public class OdbcStoreClient : IOdbcStoreClient
    {
        /// <summary>
        /// Get the warehouse customers odbc stores where key is the warehouse store id
        /// </summary>
        public IDictionary<Guid, Store> GetStores()
        {
            throw new NotImplementedException("TODO");
        }

        /// <summary>
        /// Get the given warehouseStoreIds import map
        /// </summary>
        public IOdbcFieldMap GetImportMap(Guid warehouseStoreId)
        {
            throw new NotImplementedException("TODO");
        }

        /// <summary>
        /// Get the given warehouseStoreIds upload map
        /// </summary>
        public IOdbcFieldMap GetUploadMap(Guid warehouseStoreId)
        {
            throw new NotImplementedException("TODO");
        }
    }

}
