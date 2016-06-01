using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// A filter has been changed
    /// </summary>
    public class FilterNodeEditedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FilterNodeEditedMessage(object sender, FilterNodeEntity filterNode)
        {
            Sender = sender;
            FilterNode = filterNode;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Filter that has been changed
        /// </summary>
        public FilterNodeEntity FilterNode { get; private set; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Source of the message
        /// </summary>
        public object Sender { get; private set; }
    }
}