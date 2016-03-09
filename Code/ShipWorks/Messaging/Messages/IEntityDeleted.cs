using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// An entity was deleted
    /// </summary>
    public interface IEntityDeletedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Id of the deleted entity
        /// </summary>
        long DeletedEntityID { get; }
    }
}
