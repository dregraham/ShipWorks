using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Request that the shipping dialog be opened
    /// </summary>
    public class OpenShippingDialogWithOrdersMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenShippingDialogWithOrdersMessage(object sender, IEnumerable<long> orderIDs) :
            this(sender, orderIDs, InitialShippingTabDisplay.Shipping)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenShippingDialogWithOrdersMessage(object sender, IEnumerable<long> orderIDs, InitialShippingTabDisplay initialDisplay)
        {
            Sender = sender;
            OrderIDs = orderIDs.ToReadOnly();
            InitialDisplay = initialDisplay;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenShippingDialogWithOrdersMessage(object sender, IEnumerable<long> orderIDs, RateSelectedEventArgs rateSelectedEventArgs) :
            this(sender, orderIDs)
        {
            RateSelectedEventArgs = rateSelectedEventArgs;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Shipments to load in the dialog
        /// </summary>
        public IEnumerable<long> OrderIDs { get; }

        /// <summary>
        /// Tab that should be shown when the dialog opens
        /// </summary>
        public InitialShippingTabDisplay InitialDisplay { get; }

        /// <summary>
        /// Rate selection event args
        /// </summary>
        public RateSelectedEventArgs RateSelectedEventArgs { get; }
    }
}
