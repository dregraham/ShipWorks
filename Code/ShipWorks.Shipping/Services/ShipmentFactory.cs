using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Configuration;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Factory for creating shipments
    /// </summary>
    public class ShipmentFactory : IShipmentFactory
    {
        private readonly IShippingConfiguration shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentFactory(IShippingConfiguration shippingSettings)
        {
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Create a shipment for the given order
        /// </summary>
        public ShipmentEntity Create(OrderEntity order) => ShippingManager.CreateShipment(order);

        /// <summary>
        /// Create a shipment for a given order, if necessary
        /// </summary>
        /// <param name="order"></param>
        public bool AutoCreateIfNecessary(OrderEntity order, bool createIfNoShipments)
        {
            if (shippingSettings.ShouldAutoCreateShipment(order, createIfNoShipments))
            {
                Create(order);

                return true;
            }

            return false;
        }
    }
}
