using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Setup a Generic Module store
    /// </summary>
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.GenericModule)]
    public class GenericModuleSetup : IStoreSetup
    {
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IStoreTypeManager storeTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleSetup(IEncryptionProviderFactory encryptionProviderFactory, IStoreTypeManager storeTypeManager)
        {
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Setup the store
        /// </summary>
        public async Task<StoreEntity> Setup(StoreConfiguration config)
        {
            var storeData = config.AdditionalData["genericModule"].ToObject<GenericModuleConfiguration>();

            var encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider(storeData.Username);

            var store = new GenericModuleStoreEntity();

            store.ModuleUsername = storeData.Username;
            store.ModulePassword = encryptionProvider.Encrypt(storeData.Password);
            store.ModuleUrl = storeData.URL;
            store.ModuleOnlineStoreCode = storeData.OnlineStoreCode;

            var storeType = (GenericModuleStoreType) storeTypeManager.GetType(store);
            storeType.InitializeFromOnlineModule();

            if (store.ModuleDownloadStrategy == (int) GenericStoreDownloadStrategy.ByModifiedTime)
            {
                store.InitialDownloadDays = (int?) storeData.ImportStartDetails ?? 30;
            }
            else
            {
                store.InitialDownloadOrder = storeData.ImportStartDetails ?? 0;
            }

            return store;
        }
    }
}
