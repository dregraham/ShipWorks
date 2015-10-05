using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messages
{
    /// <summary>
    /// Indicate that enabled carriers have changed
    /// </summary>
    public class EnabledCarriersChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EnabledCarriersChangedMessage(object sender, List<int> typesAdded, List<int> typesRemoved)
        {
            Sender = sender;
            Added = typesAdded.Cast<ShipmentTypeCode>().ToReadOnly();
            Removed = typesRemoved.Cast<ShipmentTypeCode>().ToReadOnly();
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Shipment types that were added
        /// </summary>
        public IEnumerable<ShipmentTypeCode> Added { get; private set; }

        /// <summary>
        /// Shipment types that were removed
        /// </summary>
        public IEnumerable<ShipmentTypeCode> Removed { get; private set; }
    }
}
