using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Notify listeners that the FedEx service type has changed
    /// </summary>
    public class FedExServiceTypeChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExServiceTypeChangedMessage(object sender, FedExServiceType serviceType)
        {
            Sender = sender;
            ServiceType = serviceType;
        }

        /// <summary>
        /// Control that sent the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// New service type
        /// </summary>
        public FedExServiceType ServiceType { get; private set; }
    }
}