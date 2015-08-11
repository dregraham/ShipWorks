using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    public interface IFedExNativeShipmentReply
    {
        NotificationSeverityType HighestSeverity { get; set; }
        CompletedShipmentDetail CompletedShipmentDetail { get; set; }
        Notification[] Notifications { get; set; }
        TransactionDetail TransactionDetail { get; set; }
    }
}
