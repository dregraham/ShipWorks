﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Class to sync stores from ShipWorks to the Hub
    /// </summary>
    public interface IHubStoreSynchronizer
    {
        /// <summary>
        /// Synchronize any stores that aren't currently in Hub
        /// </summary>
        Task SynchronizeStoresIfNeeded(IEnumerable<StoreConfiguration> storeConfigurations);

        /// <summary>
        /// Synchronize a store to Hub
        /// </summary>
        Task<Result> SynchronizeStore(StoreEntity store);

        /// <summary>
        /// Synchronize a store to the Hub with an action
        /// </summary>
        Task<Result> SynchronizeStore(StoreEntity store, ActionConfiguration actionConfiguration);
    }
}
