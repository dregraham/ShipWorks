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
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IDownloadStartingPoint downloadStartingPoint;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorStoreDtoFactory(IDownloadStartingPoint downloadStartingPoint,
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
            var storeEntity = baseStoreEntity as ChannelAdvisorStoreEntity;
            var store = helpers.PopulateCommonData(storeEntity, new ChannelAdvisorStore());

            string refreshToken = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor").Decrypt(storeEntity.RefreshToken);

            store.RefreshToken = await helpers.EncryptSecret(refreshToken)
                .ConfigureAwait(false);
            store.CountryCode = storeEntity.CountryCode;
            store.ItemAttributesToImport = storeEntity.ParsedAttributesToDownload;
            store.DaysBack = storeEntity.InitialDownloadDays ?? 1;

            return store;
        }
    }
}