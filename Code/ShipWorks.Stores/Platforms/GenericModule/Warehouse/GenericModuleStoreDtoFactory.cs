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
            GenericModuleStoreEntity storeEntity = baseStoreEntity as GenericModuleStoreEntity;
            string decryptedPassword = encryptionProviderFactory.
                CreateSecureTextEncryptionProvider(storeEntity.ModuleUsername)
                .Decrypt(storeEntity.ModulePassword);

            var store = new GenericModuleStore
            {
                Url = storeEntity.ModuleUrl,
                Username = storeEntity.ModuleUsername,
                Password = await helpers.EncryptSecret(decryptedPassword).ConfigureAwait(false),
                ImportStartDetails = (ulong?) (storeEntity.InitialDownloadDays ?? storeEntity.InitialDownloadOrder),
                OnlineStoreCode = storeEntity.ModuleOnlineStoreCode,
            };

            return helpers.PopulateCommonData(storeEntity, store);
        }
    }
}