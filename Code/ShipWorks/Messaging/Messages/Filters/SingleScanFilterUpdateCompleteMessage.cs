using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Messaging.Messages.Filters
{
    /// <summary>
    /// Message to notify that a filter counts have bee updated
    /// </summary>
    public class SingleScanFilterUpdateCompleteMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SingleScanFilterUpdateCompleteMessage(object sender, IFilterNodeContentEntity filterNodeContent, IEnumerable<long?> orderIds)
        {
            Sender = sender;
            FilterNodeContent = filterNodeContent;
            MessageId = Guid.NewGuid();
            OrderIds = orderIds;
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
        /// ObjectIds associated with filter
        /// </summary>
        public IEnumerable<long?> OrderIds { get; }
    }
}
