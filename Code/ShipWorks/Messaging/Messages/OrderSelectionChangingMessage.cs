using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            OrderIdList = new ReadOnlyCollection<long>(orderIdList.ToList());
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