using System.Collections.Generic;
using System.Xml;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Interface for connecting to ThreeDCart
    /// </summary>
    public interface IThreeDCartSoapWebClient
    {
        /// <summary>
        /// Get the list of 3dcart online order statuses
        /// </summary>
        List<ThreeDCartOrderStatus> OrderStatuses { get; }

        /// <summary>
        /// Make a call to ThreeDCart requesting a count of orders matching criteria.
        /// </summary>
        /// <param name="orderSearchCriteria">Filter by ThreeDCartWebClientOrderSearchCriteria.</param>
        /// <returns>Number of orders matching criteria</returns>
        int GetOrderCount(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria);

        /// <summary>
        /// Make the call to ThreeDCart to get a list of orders matching criteria
        /// </summary>
        /// <param name="orderSearchCriteria">Filter by ThreeDCartWebClientOrderSearchCriteria.</param>
        /// <returns>List of orders matching criteria, sorted by LastUpdate ascending </returns>
        List<XmlNode> GetOrders(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria);

        /// <summary>
        /// Gets a ThreeDCartProductDTO entry from the product cache.  If it doesn't exist, it tries to find the product
        /// online, and store it in the cache if found.  If it can't be found online, null is returned.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number for the order </param>
        /// <param name="threeDCartOrderItemProductId">The 3dcart order item product ID</param>
        /// <param name="threeDCartOrderItemName">The 3dcart order item option value text</param>
        /// <returns>ThreeDCartProductDTO representing the cart product.  Null if an online product can't be found.</returns>
        ThreeDCartProductDTO GetProduct(long invoiceNumber, string threeDCartOrderItemProductId, string threeDCartOrderItemName);

        /// <summary>
        /// Attempt to get an order count to test connecting to ThreeDCart.  If any error, assume connection failed.
        /// </summary>
        void TestConnection();

        /// <summary>
        /// Updates the online status of orders
        /// </summary>
        IResult UpdateOrderStatus(long orderNumber, string orderNumberComplete, int statusCode);

        /// <summary>
        /// Update the online status and details of the given shipment
        /// </summary>
        IResult UploadOrderShipmentDetails(ThreeDCartOnlineUpdatingOrderDetail orderDetail, long threeDCartShipmentID, string trackingNumber);
    }
}