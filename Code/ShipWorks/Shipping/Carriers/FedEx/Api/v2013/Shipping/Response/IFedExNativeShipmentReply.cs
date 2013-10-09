using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Response
{
    public interface IFedExNativeShipmentReply
    {
        NotificationSeverityType HighestSeverity { get; set; }
        CompletedShipmentDetail CompletedShipmentDetail { get; set; }
        Notification[] Notifications { get; set; }
        TransactionDetail TransactionDetail { get; set; }
    }
}
