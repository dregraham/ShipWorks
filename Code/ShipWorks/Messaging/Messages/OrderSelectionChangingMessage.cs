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
            this(sender, orderIdList, EntityGridRowSelector.First)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSelectionChangingMessage(object sender, IEnumerable<long> orderIdList, IEntityGridRowSelector shipmentSelector)
        {
            Sender = sender;
            OrderIdList = orderIdList.ToReadOnly();
            ShipmentSelector = shipmentSelector;
            MessageId = Guid.NewGuid();
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
        /// Get the shipment selector to use
        /// </summary>
        public IEntityGridRowSelector ShipmentSelector { get; }
    }
}