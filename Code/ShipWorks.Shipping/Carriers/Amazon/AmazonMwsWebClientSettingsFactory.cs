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
        private readonly IOrderManager orderManager;
        private readonly IStoreManager storeManager;

        public AmazonMwsWebClientSettingsFactory(IOrderManager orderManager, IStoreManager storeManager)
        {
            this.orderManager = orderManager;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonShipmentEntity
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public AmazonMwsWebClientSettings Create(AmazonShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
                        
            orderManager.PopulateOrderDetails(shipment.Shipment);

            long storeId = shipment.Shipment.Order.StoreID;

            StoreEntity storeEntity = storeManager.GetStore(storeId);

            return Create((AmazonStoreEntity)storeEntity);
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonStoreEntity
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public AmazonMwsWebClientSettings Create(AmazonStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            return new AmazonMwsWebClientSettings(new AmazonMwsConnection(store.MerchantID, store.AuthToken, store.AmazonApiRegion));
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonStoreEntity
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public AmazonMwsWebClientSettings Create(string merchantId, string authToken, string apiRegion)
        {
            if (string.IsNullOrWhiteSpace(merchantId) &&
                string.IsNullOrWhiteSpace(authToken) &&
                string.IsNullOrWhiteSpace(apiRegion))
            {
                throw new ArgumentException("Cannot pass empty string to Amazon Mws Connection");
            }

            return new AmazonMwsWebClientSettings(new AmazonMwsConnection(merchantId, authToken, apiRegion));
        }
    }
}
