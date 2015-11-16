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
        public IAmazonMwsWebClientSettings Create(AmazonShipmentEntity amazonShipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonShipment, nameof(amazonShipment));

            IAmazonCredentials amazonCredentials = (IAmazonCredentials)storeManager.GetRelatedStore(amazonShipment.Shipment);

            return new AmazonMwsWebClientSettings(amazonCredentials);
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an IAmazonCredentials store
        /// </summary>
        public IAmazonMwsWebClientSettings Create(IAmazonCredentials amazonCredentials)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonCredentials, nameof(amazonCredentials));

            return new AmazonMwsWebClientSettings(amazonCredentials);
        }
    }
}
