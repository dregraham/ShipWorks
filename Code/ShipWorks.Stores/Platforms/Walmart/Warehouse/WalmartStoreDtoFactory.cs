using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
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
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartStoreDtoFactory(IStoreDtoHelpers helpers,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.helpers = helpers;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as WalmartStoreEntity;
            var store = helpers.PopulateCommonData(storeEntity, new WalmartStore());

            IEncryptionProvider encryptionProvider = encryptionProviderFactory.CreateWalmartEncryptionProvider();
            string decryptedClientSecret = encryptionProvider.Decrypt(storeEntity.ClientSecret);

            store.ClientId = storeEntity.ClientID;
            store.ClientSecret = await helpers.EncryptSecret(decryptedClientSecret).ConfigureAwait(false);
            store.DownloadNumberOfDaysBack = storeEntity.DownloadModifiedNumberOfDaysBack;

            return store;
        }
    }
}
