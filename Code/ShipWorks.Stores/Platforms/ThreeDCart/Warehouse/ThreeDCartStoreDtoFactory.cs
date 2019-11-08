using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.ThreeDCart)]
    public class ThreeDCartStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartStoreDtoFactory(IStoreDtoHelpers helpers)
        {
            this.helpers = helpers;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as ThreeDCartStoreEntity;
            var store = helpers.PopulateCommonData(storeEntity, new ThreeDCartStore());

            store.Url = storeEntity.StoreUrl;
            store.Token = await helpers.EncryptSecret(storeEntity.ApiUserKey).ConfigureAwait(false);
            store.DownloadNumberOfDaysBack = storeEntity.DownloadModifiedNumberOfDaysBack;
            store.InitialDownloadDays = storeEntity.InitialDownloadDays ?? 30;
            store.TimeZoneId = storeEntity.TimeZoneID;

            return store;
        }
    }
}
