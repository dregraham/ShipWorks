using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using ShipWorks.Stores.Content.Panels.Selectors;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// The order selection on the main grid is changing
    /// </summary>
    public struct OrderSelectionChangingMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSelectionChangingMessage(object sender, IEnumerable<long> orderIdList) :
            this(sender, orderIdList, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Currently, MapPanel (and possibly other consumers of this message) expect this message to 
        /// be sent on the UI thread.
        /// </remarks>
        public OrderSelectionChangingMessage(object sender, IEnumerable<long> orderIdList, IEntityGridRowSelector shipmentSelector)
        {
            Sender = sender;
            OrderIdList = orderIdList.ToReadOnly();
            MessageId = Guid.NewGuid();
            ShipmentSelector = shipmentSelector;
        }

        /// <summary>
        /// Get the sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Get the list of order ids
        /// </summary>
        public IEnumerable<long> OrderIdList { get; }

        /// <summary>
        /// Shipment selector
        /// </summary>
        public IEntityGridRowSelector ShipmentSelector { get; }
    }
}