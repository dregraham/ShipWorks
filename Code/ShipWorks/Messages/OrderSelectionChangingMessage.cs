using ShipWorks.Core.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Core.Messages
{
    /// <summary>
    /// The order selection on the main grid is changing
    /// </summary>
    public struct OrderSelectionChangingMessage : IShipWorksMessage
    {
        private readonly IEnumerable<long> orderIdList;
        private readonly object sender;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSelectionChangingMessage(object sender, IEnumerable<long> orderIdList)
        {
            this.sender = sender;
            this.orderIdList = new ReadOnlyCollection<long>(orderIdList.ToList());
        }

        /// <summary>
        /// Get the sender of the message
        /// </summary>
        public object Sender => sender;

        /// <summary>
        /// Get the list of order ids
        /// </summary>
        public IEnumerable<long> OrderIdList => orderIdList;
    }
}