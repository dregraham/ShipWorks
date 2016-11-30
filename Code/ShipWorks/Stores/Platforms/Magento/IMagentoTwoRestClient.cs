using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Platforms.Magento.DTO;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

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
        IOrdersResponse GetOrders(DateTime? start, int currentPage);

        /// <summary>
        /// Gets a single Magento order with detailed information (attributes)
        /// </summary>
        IOrder GetOrder(long magentoOrderId);

        /// <summary>
        /// Gets a token for the given username/password
        /// </summary>
        string GetToken();

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(string shipmentDetailsJson, string invoice, long magentoOrderId);

        /// <summary>
        /// Uploads comments only
       /// </summary>
        void UploadComments(string comments, long magentoOrderID);

        /// <summary>
        /// Place a hold on a Magento order
        /// </summary>
        void HoldOrder(long magentoOrderID);

        /// <summary>
        /// Take hold off of a Magento order
        /// </summary>
        void UnholdOrder(long magentoOrderID);

        /// <summary>
        /// Cancels a Magento order
        /// </summary>
        void CancelOrder(long magentoOrderID);
    }
}