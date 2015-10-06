using System.Collections.Generic;
using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages
{
    public class OrderSelectionChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSelectionChangedMessage(object sender, List<long> orderIDs)
        {
            Sender = sender;
            OrderIDs = orderIDs;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Order IDs that have changed
        /// </summary>
        public List<long> OrderIDs { get; private set; }
    }
}
