using System;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
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
    [Component(RegisterAs = RegistrationType.ImplementedInterfaces, SingleInstance = true)]
    public class OdbcStoreRepository : IOdbcStoreRepository
    {
        private readonly ILicenseService licenseService;
        private readonly IOdbcStoreClient odbcStoreClient;
        private readonly IStoreDtoHelpers storeDtoHelpers;

        private readonly LruCache<OdbcStoreEntity, OdbcStore> storeCache =
            new LruCache<OdbcStoreEntity, OdbcStore>(1000, TimeSpan.FromMinutes(15));

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreRepository(ILicenseService licenseService, IOdbcStoreClient odbcStoreClient, IStoreDtoHelpers storeDtoHelpers)
        {
            this.licenseService = licenseService;
            this.odbcStoreClient = odbcStoreClient;
            this.storeDtoHelpers = storeDtoHelpers;

            storeCache.CacheItemRemoved += async (sender, args) => await UpdateStoreCache(args.Key);
        }

        /// <summary>
        /// Get the odbc store data for the given store
        /// </summary>
        public OdbcStore GetStore(OdbcStoreEntity store)
        {
            return WarehouseUser ?
                storeCache[store] :
                storeDtoHelpers.PopulateCommonData(store, new OdbcStore());
        }

        /// <summary>
        /// Update the cached odbc store data for the given store
        /// </summary>
        public async Task UpdateStoreCache(OdbcStoreEntity store)
        {
            if (WarehouseUser && store.WarehouseStoreID.HasValue)
            {
                Store storeData = storeDtoHelpers.PopulateCommonData(store, new Store());

                GenericResult<OdbcStore> getStoreResult = await odbcStoreClient
                    .GetStore(store.WarehouseStoreID.Value, storeData).ConfigureAwait(false);

                if (getStoreResult.Success)
                {
                    storeCache[store] = storeDtoHelpers.PopulateCommonData(store, new OdbcStore());
                }
            }
        }

        /// <summary>
        /// Whether or not the user is a warehouse customer
        /// </summary>
        private bool WarehouseUser => licenseService.CheckRestriction(EditionFeature.Warehouse, null) == EditionRestrictionLevel.None;
    }
}
