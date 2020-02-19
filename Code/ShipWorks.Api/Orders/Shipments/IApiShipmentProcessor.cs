using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// Represents the Api ShipmentProcessor
    /// </summary>
    public interface IApiShipmentProcessor
    {
        /// <summary>
        /// Process the given Shipment
        /// </summary>
        ProcessShipmentResult Process(ShipmentEntity shipment);
    }
}
