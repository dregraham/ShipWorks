using Autofac.Features.Indexed;

namespace ShipWorks.Shipping.Services.Builders
{
    /// <summary>
    /// Returns a ShipmentServiceBuilder
    /// </summary>
    public class ShipmentServicesBuilderFactory : IShipmentServicesBuilderFactory
    {
        private readonly IShipmentServicesBuilder defaultBuilder;
        private readonly IIndex<ShipmentTypeCode, IShipmentServicesBuilder> lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentServicesBuilderFactory"/> class.
        /// </summary>
        public ShipmentServicesBuilderFactory(IIndex<ShipmentTypeCode, IShipmentServicesBuilder> lookup, IShipmentServicesBuilder defaultBuilder)
        {
            this.lookup = lookup;
            this.defaultBuilder = defaultBuilder;
        }

        /// <summary>
        /// Gets the ShipmentServicesBuilder based on shipment type code.
        /// </summary>
        public IShipmentServicesBuilder Get(ShipmentTypeCode shipmentTypeCode)
        {
            IShipmentServicesBuilder builder = null;
            if (lookup.TryGetValue(shipmentTypeCode, out builder))
            {
                return builder;
            }

            return defaultBuilder;
        }
    }
}