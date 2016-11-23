using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Platforms.Magento.DTO;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Represents the Magento Two REST Web Client
    /// </summary>
    [Service]
    public interface IMagentoTwoRestClient
    {
        /// <summary>
        /// Gets Orders from the store using the start date
        /// </summary>
        OrdersResponse GetOrders(DateTime start, Uri storeUri, string token);

        /// <summary>
        /// Gets a token for the given username/password
        /// </summary>
        string GetToken(Uri storeUri, string username, string password);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(string shipmentDetailsJson, Uri storeUri, string token, long magentoOrderId);

        /// <summary>
        /// Place a hold on a Magento order
        /// </summary>
        void HoldOrder(Uri storeUri, string token, long magentoOrderID);

        /// <summary>
        /// Cancels a Magento order
        /// </summary>
        void CancelOrder(Uri storeUri, string token, long magentoOrderID);
    }
}