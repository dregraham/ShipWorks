using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
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
        private readonly IDownloadStartingPoint downloadStartingPoint;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly IWarehouseEncryptionService encryptionService;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreDtoFactory(IDownloadStartingPoint downloadStartingPoint, IStoreTypeManager storeTypeManager,
                               IWarehouseEncryptionService encryptionService)
        {
            this.downloadStartingPoint = downloadStartingPoint;
            this.storeTypeManager = storeTypeManager;
            this.encryptionService = encryptionService;
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
            store.AuthToken = await encryptionService.Encrypt(storeEntity.AuthToken)
                .ConfigureAwait(false);
            store.Region = storeEntity.AmazonApiRegion;
            store.ExcludeFBA = storeEntity.ExcludeFBA;
            store.AmazonVATS = storeEntity.AmazonVATS;
            store.DownloadStartDate = await downloadStartingPoint.OnlineLastModified(storeEntity)
                .ConfigureAwait(false);

            return store;
        }

        /// <summary>
        /// Adds ChannelAdvisor store data to the StoreDto
        /// </summary>
        private async Task<Store> AddChannelAdvisorStoreData(ChannelAdvisorStoreEntity storeEntity)
        {
            ChannelAdvisorStore store = new ChannelAdvisorStore();
            store.RefreshToken = await encryptionService.Encrypt(storeEntity.RefreshToken)
                .ConfigureAwait(false);
            store.CountryCode = storeEntity.CountryCode;
            store.ItemAttributesToImport = storeEntity.ParsedAttributesToDownload;
            store.DownloadStartDate =
                await downloadStartingPoint.OnlineLastModified(storeEntity).ConfigureAwait(false);

            return store;
        }
    }
}