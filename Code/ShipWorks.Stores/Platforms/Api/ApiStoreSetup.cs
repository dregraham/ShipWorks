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
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.Api)]
    public class ApiStoreSetup : IStoreSetup
    {
        private readonly ApiStoreType apiStoreType;

        public ApiStoreSetup(ApiStoreType apiStoreType)
        {
            this.apiStoreType = apiStoreType;
        }

        public StoreEntity Setup(StoreConfiguration config, Type storeType, StoreEntity existingStore)
        {
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
