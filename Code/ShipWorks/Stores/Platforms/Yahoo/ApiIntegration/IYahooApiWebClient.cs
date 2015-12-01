using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Interface for the Yahoo Api Web Client
    /// </summary>
    public interface IYahooApiWebClient
    {
        YahooResponse GetOrder(long orderID);
        YahooResponse GetOrderRange(long start);
        YahooResponse GetItem(string itemID);
        YahooResponse ValidateCredentials();
        void UploadShipmentDetails(string orderID, string trackingNumber, string shipper, string status);
        void UploadOrderStatus(string orderID, string status);
    }
}