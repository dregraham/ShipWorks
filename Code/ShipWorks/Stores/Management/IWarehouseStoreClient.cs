﻿using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Web client for store related interactions with the ShipWorks Warehouse app
    /// </summary>
    public interface IWarehouseStoreClient
    {
        /// <summary>
        /// Uploads a store to the ShipWorks Warehouse app
        /// </summary>
        Task<Result> UploadStoreToWarehouse(StoreEntity store, bool isNew);

        /// <summary>
        /// Update an existing stores credentials
        /// </summary>
        Task<Result> UpdateStoreCredentials(StoreEntity store);
    }
}
