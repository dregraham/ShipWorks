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

        /// <summary>
        /// Creates AmazonMwsClientSettings based on store
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        AmazonMwsWebClientSettings Create(AmazonStoreEntity store);

        /// <summary>
        /// Creates AmazonMwsClientSettings based on credentials
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="authToken"></param>
        /// <param name="apiRegion"></param>
        /// <returns></returns>
        AmazonMwsWebClientSettings Create(string merchantId, string authToken, string apiRegion);
    }
}
