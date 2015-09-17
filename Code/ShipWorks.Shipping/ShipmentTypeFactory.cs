using Autofac.Features.Indexed;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Retrieve a shipment type based on its code
    /// </summary>
    /// <remarks>
    /// It's easier to test when the factory is a real type rather than
    /// relying on using Autofac's IIndex implementation.</remarks>
    public class ShipmentTypeFactory : IShipmentTypeFactory
    {
        private IIndex<ShipmentTypeCode, ShipmentType> lookup;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lookup"></param>
        public ShipmentTypeFactory(IIndex<ShipmentTypeCode, ShipmentType> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Get the shipment type based on its code
        /// </summary>
        public ShipmentType GetType(ShipmentTypeCode shipmentTypeCode) => lookup[shipmentTypeCode];
    }
}
