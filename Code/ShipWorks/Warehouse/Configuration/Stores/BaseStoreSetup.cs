using System;
using Interapptive.Shared.ComponentRegistration;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Serialization;
using ShipWorks.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Base class for setting up a store from a getConfig call
    /// </summary>
    [KeyAllComponent(typeof(IStoreSetup), typeof(StoreTypeCode), new object[] { StoreTypeCode.Manual, StoreTypeCode.Odbc, StoreTypeCode.GenericFile, StoreTypeCode.ThreeDCart })]
    public class BaseStoreSetup : IStoreSetup
    {
        /// <summary>
        /// Setup the specified store type
        /// </summary>
        public virtual StoreEntity Setup(StoreConfiguration config, Type storeType) =>
            (StoreEntity) JsonConvert.DeserializeObject(config.SyncPayload, storeType, new EntityJsonSerializerSettings());
    }
}
