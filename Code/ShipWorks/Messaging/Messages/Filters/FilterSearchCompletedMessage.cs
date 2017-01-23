using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Messaging.Messages.Filters
{
    /// <summary>
    /// Message to notify that a quick search filter has completed
    /// </summary>
    public class FilterSearchCompletedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FilterSearchCompletedMessage(object sender, IFilterNodeEntity filterNode, IFilterNodeContentEntity filterNodeContent)
        {
            Sender = sender;
            FilterNodeContent = filterNodeContent;
            FilterNode = filterNode;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// The FilterNodeContent that completed
        /// </summary>
        public IFilterNodeContentEntity FilterNodeContent { get; }

        /// <summary>
        /// The IFilterNodeEntity that completed
        /// </summary>
        public IFilterNodeEntity FilterNode { get; }
    }
}
