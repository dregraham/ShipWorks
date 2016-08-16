using System;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// A store has been deleted
    /// </summary>
    public struct StoreDeletedMessage : IEntityDeletedMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreDeletedMessage(object sender, long deletedStoreId)
        {
            Sender = sender;
            DeletedEntityID = deletedStoreId;
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
        /// Id of the store that was deleted
        /// </summary>
        public long DeletedEntityID { get; }
    }
}
