using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Sets up a 3dCart store from a getConfig call
    /// </summary>
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.ThreeDCart)]
    public class ThreeDCartStoreSetup : BaseStoreSetup
    {
        /// <summary>
        /// Setup a 3DCart store based on the given config
        /// </summary>
        public override StoreEntity Setup(StoreConfiguration config, Type storeType)
        {
            var store = (ThreeDCartStoreEntity) base.Setup(config, storeType);
            store.TimeZoneID = TimeZoneInfo.Local.Id;
            return store;
        }
    }
}
