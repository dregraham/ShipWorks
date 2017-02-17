using Interapptive.Shared.Utility;
using ShipWorks.Data;
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
        private readonly IDataProvider dataProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentFactory(IShippingConfiguration shippingSettings, IDataProvider dataProvider)
        {
            this.shippingSettings = shippingSettings;
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Create a shipment for the given order
        /// </summary>
        public ShipmentEntity Create(OrderEntity order) => ShippingManager.CreateShipment(order);

        /// <summary>
        /// Creates a shipment for the given OrderID
        /// </summary>
        public ShipmentEntity Create(long orderId)
        {
            OrderEntity order = dataProvider.GetEntity(orderId) as OrderEntity;
            MethodConditions.EnsureArgumentIsNotNull(order, "order");

            return Create(order);
        }

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
