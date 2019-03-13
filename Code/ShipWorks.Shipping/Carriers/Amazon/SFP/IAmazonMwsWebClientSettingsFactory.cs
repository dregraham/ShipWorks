using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Factory for AmazonMwsWebClientSettings
    /// </summary>
    public interface IAmazonMwsWebClientSettingsFactory
    {
        /// <summary>
        /// Creates AmazonMwsClientSettings based on shipment
        /// </summary>
        IAmazonMwsWebClientSettings Create(AmazonSFPShipmentEntity shipment);

        /// <summary>
        /// Creates AmazonMwsClientSettings based on an IAmazonCredentials store
        /// </summary>
        IAmazonMwsWebClientSettings Create(IAmazonCredentials amazonCredentials);
    }
}
