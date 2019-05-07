using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreDtoFactory(IDownloadStartingPoint downloadStartingPoint, IStoreTypeManager storeTypeManager)
        {
            this.downloadStartingPoint = downloadStartingPoint;
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the given store's store type is not supported in
        /// ShipWorks warehouse mode.</exception>
        public async Task<StoreDto> Create(StoreEntity storeEntity)
        {
            StoreDto store = new StoreDto();
            store.StoreType = storeEntity.TypeCode;
            store.Identifier = storeTypeManager.GetType(storeEntity).LicenseIdentifier;

            // todo: Figure out what we want to do about encryption

            switch (storeEntity.StoreTypeCode)
            {
                case StoreTypeCode.Amazon:
                    await AddAmazonStoreData(storeEntity, store).ConfigureAwait(false);
                    break;
                case StoreTypeCode.ChannelAdvisor:
                    await AddChannelAdvisorStoreData(storeEntity, store).ConfigureAwait(false);
                    break;
                default:
                    throw new NotSupportedException($"The StoreType {EnumHelper.GetDescription(storeEntity.StoreTypeCode)} is not supported for ShipWorks Warehouse mode.");
            }

            return store;
        }

        /// <summary>
        /// Adds Amazon store data to the StoreDto
        /// </summary>
        private async Task AddAmazonStoreData(IStoreEntity storeEntity, StoreDto store)
        {
            if (storeEntity is AmazonStoreEntity amazonStoreEntity)
            {
                AmazonStoreData storeData = new AmazonStoreData();
                storeData.MerchantID = amazonStoreEntity.MerchantID;
                storeData.MarketplaceID = amazonStoreEntity.MarketplaceID;
                storeData.AuthToken = amazonStoreEntity.AuthToken;
                storeData.Region = amazonStoreEntity.AmazonApiRegion;
                storeData.ExcludeFBA = amazonStoreEntity.ExcludeFBA;
                storeData.AmazonVATS = amazonStoreEntity.AmazonVATS;
                storeData.DownloadStartDate = await downloadStartingPoint.OnlineLastModified(amazonStoreEntity)
                    .ConfigureAwait(false);

                store.StoreData = storeData;
            }
        }

        /// <summary>
        /// Adds ChannelAdvisor store data to the StoreDto
        /// </summary>
        private async Task AddChannelAdvisorStoreData(IStoreEntity storeEntity, StoreDto store)
        {
            if (storeEntity is ChannelAdvisorStoreEntity channelAdvisorStoreEntity)
            {
                ChannelAdvisorStoreData storeData = new ChannelAdvisorStoreData();
                storeData.RefreshToken = channelAdvisorStoreEntity.RefreshToken;
                storeData.CountryCode = channelAdvisorStoreEntity.CountryCode;
                storeData.ItemAttributesToImport = channelAdvisorStoreEntity.ParsedAttributesToDownload;
                storeData.DownloadStartDate =
                    await downloadStartingPoint.OnlineLastModified(channelAdvisorStoreEntity).ConfigureAwait(false);

                store.StoreData = storeData;
            }
        }
    }
}