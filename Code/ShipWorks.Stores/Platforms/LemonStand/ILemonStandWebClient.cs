using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    public interface ILemonStandWebClient
    {
        JToken GetBillingAddress(string customerID);
        JToken GetOrderInvoice(string orderID);
        JToken GetOrders(int page, string start);
        JToken GetProduct(string productID);
        JToken GetShipment(string invoiceID);
        JToken GetShippingAddress(string shipmentID);
        void UploadShipmentDetails(string trackingNumber, string shipmentID, string onlineStatus, string orderNumber);
    }
}