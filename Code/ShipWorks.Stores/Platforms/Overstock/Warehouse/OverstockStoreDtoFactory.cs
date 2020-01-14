using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;

namespace ShipWorks.Stores.Platforms.Overstock.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Overstock)]
    public class OverstockStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IStoreDtoHelpers helpers;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockStoreDtoFactory(IStoreDtoHelpers helpers, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.helpers = helpers;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as OverstockStoreEntity;
            var store = helpers.PopulateCommonData(storeEntity, new OverstockStore());

            var password = encryptionProviderFactory.CreateOverstockEncryptionProvider().Decrypt(storeEntity.Password);

            store.Username = storeEntity.Username;
            store.Password = await helpers.EncryptSecret(password).ConfigureAwait(false);

            return store;
        }
    }
}
