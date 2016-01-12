using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Interface for the Yahoo Api Web Client
    /// </summary>
    public interface IYahooApiWebClient
    {
        /// <summary>
        /// Gets an order.
        /// </summary>
        /// <param name="orderID">The Yahoo Order ID</param>
        YahooResponse GetOrder(long orderID);

        /// <summary>
        /// Gets a "page" of orders from a starting order number
        /// </summary>
        /// <param name="startingOrderNumber">The Yahoo Order ID to start from</param>
        YahooResponse GetOrderRange(long startingOrderNumber);

        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <param name="itemID">The Yahoo Item ID</param>
        YahooResponse GetItem(string itemID);

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        YahooResponse ValidateCredentials();

        /// <summary>
        /// Gets the custom order status.
        /// </summary>
        /// <param name="statusID">The status identifier.</param>
        YahooResponse GetCustomOrderStatus(int statusID);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        /// <param name="orderID">The order's Yahoo Order ID</param>
        /// <param name="trackingNumber">The tracking number to upload</param>
        /// <param name="shipper">The shipping carrier used</param>
        /// <param name="status">The order status to upload</param>
        void UploadShipmentDetails(string orderID, string trackingNumber, string shipper);

        /// <summary>
        /// Uploads the order status.
        /// </summary>
        /// <param name="orderID">The Yahoo Order ID</param>
        /// <param name="status">The order status to upload</param>
        void UploadOrderStatus(string orderID, string status);
    }
}