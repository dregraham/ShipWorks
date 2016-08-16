using System;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// A customer has been deleted
    /// </summary>
    public struct CustomerDeletedMessage : IEntityDeletedMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerDeletedMessage(object sender, long deletedCustomerId)
        {
            Sender = sender;
            DeletedEntityID = deletedCustomerId;
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
        /// Id of the customer that was deleted
        /// </summary>
        public long DeletedEntityID { get; }
    }
}
