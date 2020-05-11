using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.OrderLookup.Messages
{
    /// <summary>
    /// Message to create a shipment in Scan To Ship
    /// </summary>
    public class ScanToShipCreateShipmentMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipCreateShipmentMessage(object sender, long orderID)
        {
            Sender = sender;
            OrderID = orderID;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// The orderID to create a shipment for
        /// </summary>
        public long OrderID { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }
    }
}
