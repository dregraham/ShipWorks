using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;

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
        public OrderSelectionChangingMessage(object sender, IEnumerable<long> orderIdList)
        {
            Sender = sender;
            OrderIdList = orderIdList.ToReadOnly();
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
    }
}