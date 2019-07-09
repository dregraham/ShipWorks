using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.GenericModule)]
    [Component(RegistrationType.Self)]
    public class GenericModuleStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IDownloadStartingPoint downloadStartingPoint;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleStoreDtoFactory(IDownloadStartingPoint downloadStartingPoint,
                               IEncryptionProviderFactory encryptionProviderFactory,
                               IStoreDtoHelpers helpers)
        {
            this.helpers = helpers;
            this.downloadStartingPoint = downloadStartingPoint;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the given store's store type is not supported in
        /// ShipWorks warehouse mode.</exception>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var store = await Update(new GenericModuleStore(), baseStoreEntity as GenericModuleStoreEntity).ConfigureAwait(false);
            return store;
        }

        /// <summary>
        /// Update a store DTO with data from an entity
        /// </summary>
        public async Task<T> Update<T>(T store, GenericModuleStoreEntity storeEntity) where T : GenericModuleStore
        {
            string decryptedPassword = encryptionProviderFactory.
                CreateSecureTextEncryptionProvider(storeEntity.ModuleUsername)
                .Decrypt(storeEntity.ModulePassword);

            store.Url = storeEntity.ModuleUrl;
            store.Username = storeEntity.ModuleUsername;
            store.Password = await helpers.EncryptSecret(decryptedPassword).ConfigureAwait(false);
            store.ImportStartDetails = (ulong?) (storeEntity.InitialDownloadDays ?? storeEntity.InitialDownloadOrder);
            store.OnlineStoreCode = storeEntity.ModuleOnlineStoreCode;

            return helpers.PopulateCommonData(storeEntity, store);
        }
    }
}