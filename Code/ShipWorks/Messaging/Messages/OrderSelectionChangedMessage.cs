using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;

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
        public OrderSelectionChangedMessage(object sender, IEnumerable<IOrderSelection> loadedSelection)
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
        public IEnumerable<IOrderSelection> LoadedOrderSelection { get; }
    }
}
