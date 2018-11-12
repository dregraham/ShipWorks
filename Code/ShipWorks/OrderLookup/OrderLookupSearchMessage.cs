using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Message for OrderLookup Search
    /// </summary>
    public struct OrderLookupSearchMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupSearchMessage(object sender, string searchText)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            SearchText = searchText;
        }

        /// <summary>
        /// MessageId
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// The Search Text
        /// </summary>
        public string SearchText { get; }
    }
}
