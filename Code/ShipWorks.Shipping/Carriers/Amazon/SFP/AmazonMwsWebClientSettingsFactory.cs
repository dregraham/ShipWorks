using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Creates an instance of AmazonMwsWebClientSettings
    /// </summary>
    public class AmazonMwsWebClientSettingsFactory : IAmazonMwsWebClientSettingsFactory
    {
        private readonly IStoreManager storeManager;
        private readonly Func<IAmazonCredentials, IAmazonMwsWebClientSettings> getSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsWebClientSettingsFactory(IStoreManager storeManager, Func<IAmazonCredentials, IAmazonMwsWebClientSettings> getSettings)
        {
            this.getSettings = getSettings;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an AmazonShipmentEntity
        /// </summary>
        public IAmazonMwsWebClientSettings Create(AmazonSFPShipmentEntity amazonShipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonShipment, nameof(amazonShipment));

            IAmazonCredentials amazonCredentials = (IAmazonCredentials) storeManager.GetRelatedStore(amazonShipment.Shipment);

            return getSettings(amazonCredentials);
        }

        /// <summary>
        /// Creates an instance of AmazonMwsWebClientSettings from an IAmazonCredentials store
        /// </summary>
        public IAmazonMwsWebClientSettings Create(IAmazonCredentials amazonCredentials)
        {
            MethodConditions.EnsureArgumentIsNotNull(amazonCredentials, nameof(amazonCredentials));

            return getSettings(amazonCredentials);
        }
    }
}
