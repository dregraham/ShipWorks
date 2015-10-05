namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Defines a message usable with the Messenger
    /// </summary>
    /// <remarks>Right now this is just a marker interface</remarks>
    public interface IShipWorksMessage
    {
        /// <summary>
        /// Source of the message
        /// </summary>
        object Sender { get; }
    }
}