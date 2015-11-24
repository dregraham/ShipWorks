namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public interface IYahooApiWebClient
    {
        string GetOrder(long orderID);
        string GetOrderRange(long start);
        string GetItem(string itemID);
        string ValidateCredentials();
        void UploadShipmentDetails(string orderID, string trackingNumber, string shipper, string status);
        void UploadOrderStatus(string orderID, string status);
    }
}