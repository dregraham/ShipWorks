using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Messages
{
    /// <summary>
    /// Request that the shipping dialog be opened
    /// </summary>
    public class OpenShippingDialogMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenShippingDialogMessage(object sender, IEnumerable<ShipmentEntity> shipments) :
            this(sender, shipments, InitialShippingTabDisplay.Shipping)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenShippingDialogMessage(object sender, IEnumerable<ShipmentEntity> shipments, InitialShippingTabDisplay initialDisplay)
        {
            Sender = sender;
            Shipments = shipments.ToReadOnly();
            InitialDisplay = initialDisplay;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenShippingDialogMessage(object sender, IEnumerable<ShipmentEntity> shipments, RateSelectedEventArgs rateSelectedEventArgs) :
            this(sender, shipments)
        {
            RateSelectedEventArgs = rateSelectedEventArgs;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Shipments to load in the dialog
        /// </summary>
        public IEnumerable<ShipmentEntity> Shipments { get; private set; }

        /// <summary>
        /// Tab that should be shown when the dialog opens
        /// </summary>
        public InitialShippingTabDisplay InitialDisplay { get; private set; }

        /// <summary>
        /// Rate selection event args
        /// </summary>
        public RateSelectedEventArgs RateSelectedEventArgs { get; private set; }
    }
}
