using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Configuration
{
    /// <summary>
    /// Interface for settings needed by the Shipping activities
    /// </summary>
    public interface IShippingConfiguration
    {
        /// <summary>
        /// Gets whether a new shipment should be auto-created for an order
        /// </summary>
        bool ShouldAutoCreateShipment(OrderEntity order);
    }
}
