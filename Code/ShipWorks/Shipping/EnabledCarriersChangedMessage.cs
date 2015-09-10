using System.Collections.Generic;
using Interapptive.Shared.Messaging;
using System.Linq;
using System.Collections.ObjectModel;

namespace ShipWorks.Shipping
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
            Added = new ReadOnlyCollection<ShipmentTypeCode>(typesAdded.Cast<ShipmentTypeCode>().ToList());
            Removed = new ReadOnlyCollection<ShipmentTypeCode>(typesRemoved.Cast<ShipmentTypeCode>().ToList());
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
