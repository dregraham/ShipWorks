using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse.Configuration.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Stores.Platforms.Api
{
    /// <summary>
    /// Given a config, create an API Store
    /// </summary>
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.Api)]
    public class ApiStoreSetup : BaseStoreSetup
    {
        private readonly ApiStoreType apiStoreType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiStoreSetup(ApiStoreType apiStoreType)
        {
            this.apiStoreType = apiStoreType;
        }

        /// <summary>
        /// Setup the API Store
        /// </summary>
        public override StoreEntity Setup(StoreConfiguration config, Type storeType, StoreEntity existingStore)
        {
            // When a user first adds a store, there will be no payload. When SW syncs, there will be a payload
            // and we want to sync like any other store.
            if(!string.IsNullOrWhiteSpace(config.SyncPayload))
            {
                return base.Setup(config, storeType, existingStore);
            }

            var store = (PlatformStoreEntity) apiStoreType.CreateStoreInstance();

            if (existingStore != null)
            {
                store.StoreID = existingStore.StoreID;
            }

            store.SetupComplete = true;
            store.StoreName = config.Name;
            store.WarehouseStoreID = Guid.Parse(config.Id);
            store.OrderSourceID = config.UniqueIdentifier;

            return store;
        }
    }
}
