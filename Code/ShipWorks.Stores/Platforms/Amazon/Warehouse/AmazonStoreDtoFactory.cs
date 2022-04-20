using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp.Extensions;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Amazon)]
    public class AmazonStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IDownloadStartingPoint downloadStartingPoint;
        private readonly IStoreDtoHelpers helpers;
        private readonly ICustomerLicense customerLicense;
        private readonly ITangoWebClient tangoWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonStoreDtoFactory(IDownloadStartingPoint downloadStartingPoint, IStoreDtoHelpers helpers, 
            ICustomerLicense customerLicense, ITangoWebClient tangoWebClient)
        {
            this.helpers = helpers;
            this.downloadStartingPoint = downloadStartingPoint;
            this.customerLicense = customerLicense;
            this.tangoWebClient = tangoWebClient;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the given store's store type is not supported in
        /// ShipWorks warehouse mode.</exception>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            AmazonStoreEntity storeEntity = baseStoreEntity as AmazonStoreEntity;
            AmazonStore store = helpers.PopulateCommonData(storeEntity, new AmazonStore());

            if (storeEntity != null)
            {
                store.MerchantID = storeEntity.MerchantID;
                store.MarketplaceID = storeEntity.MarketplaceID;
                store.Region = storeEntity.AmazonApiRegion;
                store.ExcludeFBA = storeEntity.ExcludeFBA;
                store.AmazonVATS = storeEntity.AmazonVATS;
                var downloadStartDate = await downloadStartingPoint.OnlineLastModified(storeEntity)
                    .ConfigureAwait(false);

                store.DownloadStartDate = helpers.GetUnixTimestampMillis(downloadStartDate);
                store.AuthToken = await helpers.EncryptSecret(storeEntity.AuthToken)
                    .ConfigureAwait(false);

                store.OrderSourceID = storeEntity.OrderSourceID;

                store.PlatformAccountId = customerLicense?.CustomerID.HasValue() == true ? 
                    customerLicense.CustomerID : 
                    tangoWebClient.GetTangoCustomerId();
            }

            return store;
        }
    }
}