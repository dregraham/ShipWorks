using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores.Platforms.Odbc.Warehouse;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Repository for odbc stores
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    [Component(RegisterAs = RegistrationType.ImplementedInterfaces, SingleInstance = true)]
    public class OdbcStoreRepository : IOdbcStoreRepository, IInitializeForCurrentSession
    {
        private readonly ILicenseService licenseService;
        private readonly IOdbcStoreClient odbcStoreClient;
        private readonly IStoreDtoHelpers storeDtoHelpers;
        private readonly IStoreManager storeManager;
        private readonly ILog log;
        private readonly TimeSpan refreshInterval = TimeSpan.FromMinutes(15);

        private readonly ConcurrentDictionary<OdbcStoreEntity, RefreshingOdbcStore> storeCache;
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreRepository(ILicenseService licenseService, IOdbcStoreClient odbcStoreClient,
                                   IStoreDtoHelpers storeDtoHelpers, IStoreManager storeManager,
                                   Func<Type, ILog> logFactory)
        {
            this.licenseService = licenseService;
            this.odbcStoreClient = odbcStoreClient;
            this.storeDtoHelpers = storeDtoHelpers;
            this.storeManager = storeManager;
            log = logFactory(typeof(OdbcStoreRepository));

            LambdaComparer<OdbcStoreEntity> storeComparer = new LambdaComparer<OdbcStoreEntity>((a, b) => a.StoreID == b.StoreID);
            storeCache = new ConcurrentDictionary<OdbcStoreEntity, RefreshingOdbcStore>(storeComparer);
        }

        /// <summary>
        /// Get the odbc store data for the given store
        /// </summary>
        public OdbcStore GetStore(OdbcStoreEntity store)
        {
            if (WarehouseUser && store.WarehouseStoreID.HasValue)
            {
                if(storeCache.TryGetValue(store, out RefreshingOdbcStore refreshingOdbcStore))
                {
                    // If there was a store in the cache return it
                    return refreshingOdbcStore.Store;
                }                

                // Since there was no store in the cache, get one from the hub
                OdbcStore storeFromHub = Task.Run(
                    async () => await GetStoreFromHub(store).ConfigureAwait(true)).Result;
 
                if (storeFromHub != null)
                {
                    // Save the store to the cache and return it
                    storeCache[store] = new RefreshingOdbcStore(storeFromHub, store, GetStoreFromHub, refreshInterval);
                    return storeFromHub;
                }

                // If we got here, we were unable to get a store from the cache or the hub. Use local store instead.
                log.Error("Failed to retrieve odbc store data from the hub. Continuing with local store");
                return storeDtoHelpers.PopulateCommonData(store, new OdbcStore());
            }

            // If not a warehouse user, just return the local odbc store data
            return storeDtoHelpers.PopulateCommonData(store, new OdbcStore());
        }

        /// <summary>
        /// Get the odbc store data from the hub
        /// </summary>
        private async Task<OdbcStore> GetStoreFromHub(OdbcStoreEntity store)
        {
            Store storeData = storeDtoHelpers.PopulateCommonData(store, new Store());

            GenericResult<OdbcStore> getStoreResult = await odbcStoreClient
                .GetStore(store.WarehouseStoreID.Value, storeData).ConfigureAwait(false);

            return getStoreResult.Success ?
                getStoreResult.Value :
                null;
        }

        /// <summary>
        /// Whether or not the user is a warehouse customer
        /// </summary>
        private bool WarehouseUser => licenseService.CheckRestriction(EditionFeature.Warehouse, null) == EditionRestrictionLevel.None;

        /// <summary>
        /// Initialize for current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            IEnumerable<OdbcStoreEntity> odbcStores = storeManager.GetEnabledStores()
                .Where(x => x.StoreTypeCode == StoreTypeCode.Odbc).Cast<OdbcStoreEntity>();

            foreach (OdbcStoreEntity odbcStore in odbcStores)
            {
                GetStore(odbcStore);
            }
        }
    }
}
