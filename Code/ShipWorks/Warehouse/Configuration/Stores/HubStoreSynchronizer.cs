using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Management;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Class to sync stores from ShipWorks to the Hub
    /// </summary>
    [Component]
    public class HubStoreSynchronizer : IHubStoreSynchronizer
    {
        private readonly IStoreManager storeManager;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly IWarehouseStoreClient warehouseStoreClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubStoreSynchronizer(IStoreManager storeManager,
            IStoreTypeManager storeTypeManager,
            IWarehouseStoreClient warehouseStoreClient)
        {
            this.storeManager = storeManager;
            this.storeTypeManager = storeTypeManager;
            this.warehouseStoreClient = warehouseStoreClient;
        }

        /// <summary>
        /// Synchronize any stores that aren't currently in Hub
        /// </summary>
        public async Task SynchronizeStoresIfNeeded(IEnumerable<StoreConfiguration> storeConfigurations)
        {
            var storeIDsToExclude = new HashSet<string>(storeConfigurations.Select(x => x.UniqueIdentifier));
            var storesToSync = storeManager.GetAllStores().Where(x => !storeIDsToExclude.Contains(storeTypeManager.GetType(x).LicenseIdentifier));

            if (storesToSync.None())
            {
                return;
            }

            await SynchronizeStores(storesToSync).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronize a store to Hub
        /// </summary>
        public async Task<Result> SynchronizeStore(StoreEntity store) =>
            await SynchronizeStores(new List<StoreEntity> { store }).ConfigureAwait(false);

        /// <summary>
        /// Synchronize a store to the Hub with an action
        /// </summary>
        public async Task<Result> SynchronizeStore(StoreEntity store, ActionConfiguration actionConfiguration) =>
            await SynchronizeStores(new List<StoreEntity> { store }, actionConfiguration).ConfigureAwait(false);

        /// <summary>
        /// Synchronize the given stores to the Hub
        /// </summary>
        private async Task<Result> SynchronizeStores(IEnumerable<StoreEntity> stores, ActionConfiguration actionConfiguration = null)
        {
            var request = new StoreSynchronizationRequest()
            {
                StoreSynchronizations = new List<StoreSynchronization>()
            };

            foreach (var store in stores)
            {
                var synchronization = new StoreSynchronization()
                {
                    Name = store.StoreName,
                    UniqueIdentifier = storeTypeManager.GetType(store).LicenseIdentifier,
                    StoreType = store.StoreTypeCode,
                    SyncPayload = JsonConvert.SerializeObject(store),
                    ActionsPayload = JsonConvert.SerializeObject(actionConfiguration)
                };

                request.StoreSynchronizations.Add(synchronization);
            }

            return await warehouseStoreClient.SynchronizeStores(request).ConfigureAwait(false);
        }
    }
}
