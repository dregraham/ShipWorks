using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Messaging.Messages.Dialogs
{
    /// <summary>
    /// Request that the shipping dialog be opened
    /// </summary>
    public struct OpenShippingDialogMessage : IShipWorksMessage
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
            MessageId = Guid.NewGuid();
            RateSelectedEventArgs = null;
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
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Shipments to load in the dialog
        /// </summary>
        public IEnumerable<ShipmentEntity> Shipments { get; }

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
