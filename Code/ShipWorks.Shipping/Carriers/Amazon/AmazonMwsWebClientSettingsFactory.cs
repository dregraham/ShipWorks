using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using System;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Creates an instance of AmazonMwsWebClientSettings
    /// </summary>
    public class AmazonMwsWebClientSettingsFactory : IAmazonMwsWebClientSettingsFactory
    {
        private readonly IStoreManager storeManager;

        public AmazonMwsWebClientSettingsFactory(IStoreManager storeManager)
        {
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonShipmentEntity
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public AmazonMwsWebClientSettings Create(AmazonShipmentEntity amazonShipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonShipment, nameof(amazonShipment));

            StoreEntity store = storeManager.GetRelatedStore(amazonShipment.Shipment);

            return Create((AmazonStoreEntity)store);
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonStoreEntity
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        private AmazonMwsWebClientSettings Create(AmazonStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            return new AmazonMwsWebClientSettings(new AmazonMwsConnection(store.MerchantID, store.AuthToken, store.AmazonApiRegion));
        }
    }
}
