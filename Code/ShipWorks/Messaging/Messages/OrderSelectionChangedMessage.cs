using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Stores.Content.Panels.Selectors;

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
        public OrderSelectionChangedMessage(object sender, IEnumerable<IOrderSelection> loadedSelection) :
            this(sender, loadedSelection, EntityGridRowSelector.First)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSelectionChangedMessage(object sender,
            IEnumerable<IOrderSelection> loadedSelection, IEntityGridRowSelector shipmentSelector)
        {
            Sender = sender;
            LoadedOrderSelection = loadedSelection.ToReadOnly();
            ShipmentSelector = shipmentSelector;
            MessageId = Guid.NewGuid();
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
        /// Order IDs that have changed
        /// </summary>
        public IEnumerable<IOrderSelection> LoadedOrderSelection { get; }

        /// <summary>
        /// Shipments that are selected
        /// </summary>
        public IEntityGridRowSelector ShipmentSelector { get; }
    }
}
