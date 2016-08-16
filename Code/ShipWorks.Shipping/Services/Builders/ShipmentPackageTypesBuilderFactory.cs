using Autofac.Features.Indexed;

namespace ShipWorks.Shipping.Services.Builders
{
    /// <summary>
    /// Implementation of IShipmentPackageTypesBuilderFactory
    /// </summary>
    public class ShipmentPackageTypesBuilderFactory : IShipmentPackageTypesBuilderFactory
    {
        private readonly IIndex<ShipmentTypeCode, IShipmentPackageTypesBuilder> lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentPackageTypesBuilderFactory"/> class.
        /// </summary>
        public ShipmentPackageTypesBuilderFactory(IIndex<ShipmentTypeCode, IShipmentPackageTypesBuilder> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Gets the IShipmentPackageTypesBuilder based on shipment type code.
        /// </summary>
        public IShipmentPackageTypesBuilder Get(ShipmentTypeCode shipmentTypeCode) =>
            lookup[shipmentTypeCode];
    }
}