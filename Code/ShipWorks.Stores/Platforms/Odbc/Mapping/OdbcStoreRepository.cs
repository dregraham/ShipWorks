using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
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
        private readonly IStoreManager storeManager;
        private readonly ILog log;
        private readonly IStoreDtoFactory odbcStoreDtoFactory;
        private readonly TimeSpan refreshInterval = TimeSpan.FromMinutes(15);

        private readonly ConcurrentDictionary<OdbcStoreEntity, RefreshingOdbcStore> storeCache;
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreRepository(ILicenseService licenseService, IOdbcStoreClient odbcStoreClient,
                                   IStoreManager storeManager,
                                   IIndex<StoreTypeCode, IStoreDtoFactory> storeDtoFactories,
                                   Func<Type, ILog> logFactory)
        {
            this.licenseService = licenseService;
            this.odbcStoreClient = odbcStoreClient;
            this.storeManager = storeManager;
            log = logFactory(typeof(OdbcStoreRepository));
            odbcStoreDtoFactory = storeDtoFactories[StoreTypeCode.Odbc];

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
                    async () => await GetStoreFromHub(store).ConfigureAwait(false)).Result;
 
                if (storeFromHub != null)
                {
                    // Save the store to the cache and return it
                    storeCache[store] = new RefreshingOdbcStore(storeFromHub, store, GetStoreFromHub, refreshInterval);
                    return storeFromHub;
                }

                // If we got here, we were unable to get a store from the cache or the hub.
                log.Error("Failed to retrieve odbc store data from the hub.");

                throw new ShipWorksOdbcException("Failed to retrieve store from the hub.");
            }

            // If not a warehouse user, just return the local odbc store data
            return (OdbcStore) Task.Run(async () => await odbcStoreDtoFactory.Create(store).ConfigureAwait(false)).Result;
        }

        /// <summary>
        /// Update the cached odbc store data for the given store
        /// </summary>
        public async Task UpdateStoreCache(OdbcStoreEntity store)
        {
            if (WarehouseUser && store.WarehouseStoreID.HasValue)
            {
                OdbcStore storeDto = (OdbcStore) await odbcStoreDtoFactory.Create(store).ConfigureAwait(false);

                // Get the old store, if there is one
                RefreshingOdbcStore oldStore;
                storeCache.TryGetValue(store, out oldStore);

                // Add or overwrite the value of store
                storeCache[store] = new RefreshingOdbcStore(storeDto, store, GetStoreFromHub, refreshInterval);

                // delete the old store, if found.
                oldStore?.Dispose();
            }
        }

        /// <summary>
        /// Get the odbc store data from the hub
        /// </summary>
        private async Task<OdbcStore> GetStoreFromHub(OdbcStoreEntity store)
        {
            Store storeData = await odbcStoreDtoFactory.Create(store).ConfigureAwait(false);

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
                try
                {
                    GetStore(odbcStore);
                }
                catch (ShipWorksOdbcException ex)
                {
                    // Just logging and eating the error. They will try again when
                    // downloading or uploading and that is an easier time to handle the error
                    log.Error(ex);
                }
            }
        }
    }
}
