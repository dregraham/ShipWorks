namespace ShipWorks.Core.Messaging.Messages.Shipping
{
    /// <summary>
    /// Basic order selection with nothing loaded
    /// </summary>
    public struct BasicOrderSelection : IOrderSelection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BasicOrderSelection(long orderID)
        {
            OrderID = orderID;
        }

        /// <summary>
        /// Id of the order selection
        /// </summary>
        public long OrderID { get; }
    }
}
