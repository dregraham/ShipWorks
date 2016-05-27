using System;
using Interapptive.Shared.Messaging;
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
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// New service type
        /// </summary>
        public FedExServiceType ServiceType { get; }
    }
}