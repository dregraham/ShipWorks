using System.Collections.Generic;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using Interapptive.Shared.Collections;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Order selection change loaded complete
    /// </summary>
    public struct OrderSelectionChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSelectionChangedMessage(object sender, IEnumerable<OrderSelectionLoaded> loadedSelection)
        {
            Sender = sender;
            LoadedOrderSelection = loadedSelection.ToReadOnly();
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Order IDs that have changed
        /// </summary>
        public IEnumerable<OrderSelectionLoaded> LoadedOrderSelection { get; }
    }
}
