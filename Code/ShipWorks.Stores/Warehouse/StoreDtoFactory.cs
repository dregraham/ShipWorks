using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Ebay;
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
                case EbayStoreEntity ebayStore:
                    store = await AddEbayStoreData(ebayStore).ConfigureAwait(false);
                    break;
                case GenericModuleStoreEntity genericModuleStore:
                    store = await AddGenericModuleStoreData(genericModuleStore).ConfigureAwait(false);
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
        /// Adds GenericModule store data to the StoreDto
        /// </summary>
        private async Task<Store> AddGenericModuleStoreData(GenericModuleStoreEntity storeEntity)
        {
            string decryptedPassword = encryptionProviderFactory.CreateSecureTextEncryptionProvider(storeEntity.ModuleUsername).Decrypt(storeEntity.ModulePassword);

            return new GenericModuleStore
            {
                Url = storeEntity.ModuleUrl,
                Username = storeEntity.ModuleUsername,
                Password = await encryptionService.Encrypt(decryptedPassword).ConfigureAwait(false),
                ImportStartDetails = (ulong?) (storeEntity.InitialDownloadDays ?? storeEntity.InitialDownloadOrder),
            };
        }

        /// <summary>
        /// Adds Ebay store data to the StoreDto
        /// </summary>
        /// <returns></returns>
        private async Task<Store> AddEbayStoreData(EbayStoreEntity storeEntity)
        {
            EbayStore store = new EbayStore();

            var downloadStartDate = await downloadStartingPoint.OnlineLastModified(storeEntity)
                .ConfigureAwait(false);

            store.DownloadStartDate = GetUnixTimestampMillis(downloadStartDate);
            store.EbayToken = await encryptionService.Encrypt(storeEntity.EBayToken).ConfigureAwait(false);
            store.UseSandbox = !EbayUrlUtilities.UseLiveServer;

            return store;

        }

        /// <summary>
        /// get unix timestamp from the given date time
        /// </summary>
        private static ulong GetUnixTimestampMillis(DateTime? dateTime) =>
            Convert.ToUInt64((dateTime - UnixEpoch)?.TotalMilliseconds ?? 0);
    }
}