using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne;

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
        OrdersResponse GetOrders(DateTime? start, int currentPage);

        /// <summary>
        /// Gets a single Magento order with detailed information (attributes)
        /// </summary>
        Order GetOrder(long magentoOrderId);

        /// <summary>
        /// Gets a token for the given username/password
        /// </summary>
        string GetToken();

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(string shipment, string invoice, long magentoOrderId);

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

        /// <summary>
        /// Gets the specified item
        /// </summary>
        /// <returns></returns>
        Item GetItem(long itemId);

        /// <summary>
        /// Gets the product for the given sku
        /// </summary>
        /// <param name="sku"></param>
        /// <returns>
        /// Product is similar to item except it contains product details
        /// and is not order specific details like option names and image urls
        /// </returns>
        Product GetProduct(string sku);
    }
}