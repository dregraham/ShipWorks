using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Warehouse.Encryption;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [Component(RegistrationType.Self)]
    public class StoreDtoFactory
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly IDownloadStartingPoint downloadStartingPoint;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly IWarehouseEncryptionService encryptionService;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreDtoFactory(IDownloadStartingPoint downloadStartingPoint, IStoreTypeManager storeTypeManager,
                               IWarehouseEncryptionService encryptionService, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.downloadStartingPoint = downloadStartingPoint;
            this.storeTypeManager = storeTypeManager;
            this.encryptionService = encryptionService;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the given store's store type is not supported in
        /// ShipWorks warehouse mode.</exception>
        public async Task<Store> Create(StoreEntity storeEntity)
        {
            Store store;

            switch (storeEntity)
            {
                case AmazonStoreEntity amazonStore:
                    store = await AddAmazonStoreData(amazonStore).ConfigureAwait(false);
                    break;
                case ChannelAdvisorStoreEntity channelAdvisorStore:
                    store = await AddChannelAdvisorStoreData(channelAdvisorStore).ConfigureAwait(false);
                    break;
                default:
                    throw new NotSupportedException($"The StoreType {EnumHelper.GetDescription(storeEntity.StoreTypeCode)} is not supported for ShipWorks Warehouse mode.");
            }

            store.Name = storeEntity.StoreName;
            store.StoreType = storeEntity.TypeCode;
            store.UniqueIdentifier = storeTypeManager.GetType(storeEntity).LicenseIdentifier;

            return store;
        }

        /// <summary>
        /// Adds Amazon store data to the StoreDto
        /// </summary>
        private async Task<Store> AddAmazonStoreData(AmazonStoreEntity storeEntity)
        {
            AmazonStore store = new AmazonStore();
            store.MerchantID = storeEntity.MerchantID;
            store.MarketplaceID = storeEntity.MarketplaceID;
            store.Region = storeEntity.AmazonApiRegion;
            store.ExcludeFBA = storeEntity.ExcludeFBA;
            store.AmazonVATS = storeEntity.AmazonVATS;
            var downloadStartDate = await downloadStartingPoint.OnlineLastModified(storeEntity)
                .ConfigureAwait(false);

            store.DownloadStartDate = GetUnixTimestampMillis(downloadStartDate);
            store.AuthToken = await encryptionService.Encrypt(storeEntity.AuthToken)
                                .ConfigureAwait(false);
            return store;
        }

        /// <summary>
        /// Adds ChannelAdvisor store data to the StoreDto
        /// </summary>
        private async Task<Store> AddChannelAdvisorStoreData(ChannelAdvisorStoreEntity storeEntity)
        {
            string refreshToken = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor").Decrypt(storeEntity.RefreshToken);

            ChannelAdvisorStore store = new ChannelAdvisorStore();
            store.RefreshToken = await encryptionService.Encrypt(refreshToken)
                .ConfigureAwait(false);
            store.CountryCode = storeEntity.CountryCode;
            store.ItemAttributesToImport = storeEntity.ParsedAttributesToDownload;
            var downloadStartDate = await downloadStartingPoint.OnlineLastModified(storeEntity).ConfigureAwait(false);
            store.DownloadStartDate = GetUnixTimestampMillis(downloadStartDate);

            return store;
        }

        /// <summary>
        /// get unix timestamp from the given date time
        /// </summary>
        private static ulong GetUnixTimestampMillis(DateTime? dateTime) =>
            Convert.ToUInt64((dateTime - UnixEpoch)?.TotalMilliseconds ?? 0);
    }
}