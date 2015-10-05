using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messages
{
    /// <summary>
    /// An IShipWorksMessage used to inform subscribers that a store has changed.
    /// </summary>
    public class StoreChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreChangedMessage(object sender, StoreEntity store)
        {
            Sender = sender;
            StoreEntity = store;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Store that has changed
        /// </summary>
        public StoreEntity StoreEntity { get; private set; }
    }
}
