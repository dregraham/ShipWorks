﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Interface for store-specific setup classes
    /// </summary>
    public interface IStoreSetup
    {
        /// <summary>
        /// Setup a store from given store configuration
        /// </summary>
        StoreEntity Setup(StoreConfiguration config, Type storeType, StoreEntity existingStore);
    }
}
