using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;

namespace ShipWorks.Stores.Platforms.Walmart.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Walmart)]
    public class WalmartStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartStoreDtoFactory(IStoreDtoHelpers helpers)
        {
            this.helpers = helpers;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as WalmartStoreEntity;
            var store = helpers.PopulateCommonData(storeEntity, new WalmartStore());

            store.ClientId = storeEntity.ClientID;
            store.ClientSecret = await helpers.EncryptSecret(storeEntity.ClientSecret).ConfigureAwait(false);
            store.DownloadNumberOfDaysBack = storeEntity.DownloadModifiedNumberOfDaysBack;

            return store;
        }
    }
}
