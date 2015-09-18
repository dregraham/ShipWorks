using Autofac.Features.Indexed;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Returns a ShipmentServiceBuilder
    /// </summary>
    class ShipmentServicesBuilderFactory : IShipmentServicesBuilderFactory
    {
        private readonly IIndex<ShipmentTypeCode, IShipmentServicesBuilder> lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentServicesBuilderFactory"/> class.
        /// </summary>
        public ShipmentServicesBuilderFactory(IIndex<ShipmentTypeCode, IShipmentServicesBuilder> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Gets the ShipmentServicesBuilder based on shipment type code.
        /// </summary>
        public IShipmentServicesBuilder Get(ShipmentTypeCode shipmentTypeCode) =>
                    lookup[shipmentTypeCode];
    }
}