using System;
using System.Collections.Generic;
using Interapptive.Shared.Threading;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Interface for connecting to BigCommerce
    /// </summary>
    public interface IBigCommerceWebClient
    {
        /// <summary>
        /// Progress reporter associated with the client
        /// </summary>
        /// <remarks>
        /// If this is null, the client cannot be canceled and progress will not be reported
        /// </remarks>
        IProgressReporter ProgressReporter { get; set; }

        /// <summary>
        /// Attempt to get an order count to test connecting to BigCommerce.  If any error, assume connection failed.
        /// </summary>
        /// <exception cref="BigCommerceException" />
        void TestConnection();

        /// <summary>
        /// Make a call to BigCommerce requesting a count of orders matching criteria.
        /// </summary>
        /// <param name="orderSearchCriteria">Get order count based on BigCommerceWebClientOrderSearchCriteria.</param>
        /// <returns>Number of orders matching criteria</returns>
        /// <exception cref="BigCommerceException" />
        int GetOrderCount(BigCommerceWebClientOrderSearchCriteria orderSearchCriteria);

        /// <summary>
        /// Make the call to BigCommerce to get a list of orders matching criteria
        /// </summary>
        /// <param name="orderSearchCriteria">Filter by BigCommerceWebClientOrderSearchCriteria.</param>
        /// <returns>List of orders matching criteria, sorted by LastUpdate ascending </returns>
        List<BigCommerceOrder> GetOrders(BigCommerceWebClientOrderSearchCriteria orderSearchCriteria);

        /// <summary>
        /// Get the list of BigCommerce online order statuses
        /// </summary>
        IEnumerable<BigCommerceOrderStatus> FetchOrderStatuses();

        /// <summary>
        /// Updates the online status of orders
        /// </summary>
        /// <exception cref="BigCommerceException" />
        void UpdateOrderStatus(int orderNumber, int statusCode);

        /// <summary>
        /// Get a list of Order Products for the order
        /// </summary>
        List<BigCommerceProduct> GetOrderProducts(long orderNumber);

        /// <summary>
        /// Update the online status and details of the given shipment
        /// </summary>
        /// <param name="orderNumber">The order number of this shipment</param>
        /// <param name="orderAddressID">The BigCommerce order addressID for this shipment</param>
        /// <param name="trackingNumber">Tracking number for this shipment</param>
        /// <param name="shippingMethod">Carrier and service for this shipment</param>
        /// <param name="orderItems">The list of BigCommerceItem's in this shipment</param>
        /// <exception cref="BigCommerceException" />
        void UploadOrderShipmentDetails(long orderNumber, long bigCommerceOrderAddressId, string trackingNumber, 
            Tuple<string, string> shippingMethod, List<BigCommerceItem> bigCommerceOrderItems);
    }
}