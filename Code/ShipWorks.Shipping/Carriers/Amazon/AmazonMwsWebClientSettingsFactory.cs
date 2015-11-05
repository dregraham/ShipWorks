using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;

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
        public AmazonMwsWebClientSettings Create(AmazonShipmentEntity amazonShipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonShipment, nameof(amazonShipment));

            IAmazonCredentials amazonCredentials = (IAmazonCredentials)storeManager.GetRelatedStore(amazonShipment.Shipment);
            
            return new AmazonMwsWebClientSettings(amazonCredentials);
        }
    }
}
