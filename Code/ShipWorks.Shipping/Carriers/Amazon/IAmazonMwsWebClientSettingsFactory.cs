using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Factory for AmazonMwsWebClientSettings
    /// </summary>
    public interface IAmazonMwsWebClientSettingsFactory
    {
        /// <summary>
        /// Creates AmazonMwsClientSettings based on shipment
        /// </summary>
        IAmazonMwsWebClientSettings Create(AmazonShipmentEntity shipment);
    }
}
