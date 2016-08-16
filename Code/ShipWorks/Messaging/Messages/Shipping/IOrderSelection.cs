namespace ShipWorks.Core.Messaging.Messages.Shipping
{
    /// <summary>
    /// Represents an order selection
    /// </summary>
    public interface IOrderSelection
    {
        /// <summary>
        /// Id of the order selection
        /// </summary>
        long OrderID { get; }
    }
}