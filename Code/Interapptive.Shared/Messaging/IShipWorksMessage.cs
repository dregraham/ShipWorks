namespace Interapptive.Shared.Messaging
{
    /// <summary>
    /// Defines a message usable with the Messenger
    /// </summary>
    /// <remarks>Right now this is just a marker interface</remarks>
    public interface IShipWorksMessage
    {
        object Sender { get; }
    }
}