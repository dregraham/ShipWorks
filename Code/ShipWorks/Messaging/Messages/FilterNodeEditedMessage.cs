using ShipWorks.Core.Messaging;
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
        }

        /// <summary>
        /// Filter that has been changed
        /// </summary>
        public FilterNodeEntity FilterNode { get; private set; }

        /// <summary>
        /// Source of the message
        /// </summary>
        public object Sender { get; private set; }
    }
}