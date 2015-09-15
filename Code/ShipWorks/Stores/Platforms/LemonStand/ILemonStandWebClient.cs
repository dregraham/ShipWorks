using System;
using Newtonsoft.Json.Linq;
namespace ShipWorks.Stores.Platforms.LemonStand
{
    public interface ILemonStandWebClient
    {
        JToken GetBillingAddress(string customerId);
        JToken GetOrderInvoice(string orderId);
        JToken GetOrders();
        JToken GetProduct(string productId);
        JToken GetShipment(string invoiceId);
        JToken GetShippingAddress(string shipmentId);
        void UploadShipmentDetails(string trackingNumber, string shipmentId, string onlineStatus, string orderNumber);
    }
}
