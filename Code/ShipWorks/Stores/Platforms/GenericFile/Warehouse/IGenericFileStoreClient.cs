using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.GenericFile.Warehouse
{
    /// <summary>
    /// Client for interacting with generic file stores and warehouse
    /// </summary>
    [Service]
    public interface IGenericFileStoreClient
    {
        /// <summary>
        /// Get the warehouse customers generic file stores where key is the warehouse store id
        /// </summary>
        Task<GenericResult<Dictionary<Guid, Store>>> GetStores();

        /// <summary>
        /// Get the given warehouseStoreIds OdbcStore
        /// </summary>
        Task<GenericResult<GenericFileStore>> GetStore(Guid warehouseStoreId, Store baseStore);
    }
}