using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.Configuration.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Stores.Platforms.BrightpearlHub
{
    /// <summary>
    /// Given a config, create an BrightpearlHub Store
    /// </summary>
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.BrightpearlHub)]
    public class BrightpearlHubStoreSetup : BaseStoreSetup
    {
        private readonly BrightpearlHubStoreType brightpearlHubStoreType;

        /// <summary>
        /// Constructor
        /// </summary>
        public BrightpearlHubStoreSetup(BrightpearlHubStoreType brightpearlHubStoreType)
        {
            this.brightpearlHubStoreType = brightpearlHubStoreType;
        }

        /// <summary>
        /// Setup the BrightpearlHub Store
        /// </summary>
        public override StoreEntity Setup(StoreConfiguration config, Type storeType, StoreEntity existingStore)
        {
            // When a user first adds a store, there will be no payload. When SW syncs, there will be a payload
            // and we want to sync like any other store.
            if(!string.IsNullOrWhiteSpace(config.SyncPayload))
            {
                return base.Setup(config, storeType, existingStore);
            }

            var store = (PlatformStoreEntity) brightpearlHubStoreType.CreateStoreInstance();

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
