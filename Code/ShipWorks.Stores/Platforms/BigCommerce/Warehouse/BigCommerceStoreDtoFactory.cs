using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;

namespace ShipWorks.Stores.Platforms.BigCommerce.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.BigCommerce)]
    public class BigCommerceStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceStoreDtoFactory(IStoreDtoHelpers helpers)
        {
            this.helpers = helpers;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as BigCommerceStoreEntity;
            var store = helpers.PopulateCommonData(storeEntity, new BigCommerceStore());

            store.ClientId = storeEntity.OauthClientId;
            store.AccessToken = await helpers.EncryptSecret(storeEntity.OauthToken).ConfigureAwait(false);
            store.Url = storeEntity.ApiUrl;
            store.DownloadNumberOfDaysBack = storeEntity.DownloadModifiedNumberOfDaysBack;
            store.InitialDownloadDays = storeEntity.InitialDownloadDays;

            return store;
        }
    }
}
