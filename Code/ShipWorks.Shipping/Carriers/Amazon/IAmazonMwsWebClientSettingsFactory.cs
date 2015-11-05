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
        /// <param name="shipment"></param>
        /// <returns></returns>
        AmazonMwsWebClientSettings Create(AmazonShipmentEntity shipment);
    }
}
