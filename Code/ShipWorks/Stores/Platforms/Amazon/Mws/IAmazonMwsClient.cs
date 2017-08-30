using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Handles communication with Amazon MWS (Marketplace Web Service) for
    /// retrieving orders and uploading shipment information
    /// </summary>
    public interface IAmazonMwsClient : IDisposable
    {
        /// <summary>
        /// The store the webClient is operating on behalf of
        /// </summary>
        AmazonStoreEntity Store { get; }

        /// <summary>
        /// Progress reporter that will be used for requests
        /// </summary>
        IProgressReporter Progress { get; set; }

        /// <summary>
        /// Makes an api call to make sure the MWS system is not RED (down)
        /// </summary>
        Task TestServiceStatus();

        /// <summary>
        /// Makes an api call to see if we can connect with the credentials
        /// </summary>
        Task TestCredentials();

        /// <summary>
        /// Get the list of marketplaces associated with the given merchantID
        /// </summary>
        Task<List<AmazonMwsMarketplace>> GetMarketplaces();

        /// <summary>
        /// Executes a request for more orders
        /// </summary>
        Task GetOrders(DateTime? startDate, Func<XPathNamespaceNavigator, Task<bool>> loadOrder);

        /// <summary>
        /// Retrieves an order's items
        /// </summary>
        Task GetOrderItems(string amazonOrderID, Action<XPathNamespaceNavigator> loadOrderItem);

        /// <summary>
        /// Gets additional details from Amazon for the given order items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>An XPathNamespaceNavigator object.</returns>
        Task<XPathNamespaceNavigator> GetProductDetails(List<AmazonOrderItemEntity> items);

        /// <summary>
        /// Upload shipments
        /// </summary>
        Task UploadShipmentDetails(List<AmazonOrderUploadDetail> shipments);

        /// <summary>
        /// Gets the carrier for the shipment.  If the shipment type is Other, it will use Other.Carrier.
        /// </summary>
        /// <param name="shipment">The shipment for which to get the carrier name.</param>
        /// <param name="shipmentTypeCode">The shipment type code for this shipment.</param>
        /// <returns>The carrier name of the shipment type, unless it is of type Other, then the Other.Carrier is returned.</returns>
        string GetCarrierName(ShipmentEntity shipment, ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Determines if the local system clock is in sync with Amazon's servers.
        /// ONLY fails if we receive a time from Amazon and we are for sure out of sync.
        /// </summary>
        Task<bool> ClockInSyncWithMWS();
    }
}