using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
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