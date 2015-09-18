using Autofac.Features.Indexed;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Retrieves a ShipmentPackageBuilder
    /// </summary>
    internal class ShipmentPackageBuilderFactory : IShipmentPackageBuilderFactory
    {
        private readonly IIndex<ShipmentTypeCode, IShipmentPackageTypesBuilder> lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentPackageBuilderFactory"/> class.
        /// </summary>
        public ShipmentPackageBuilderFactory(IIndex<ShipmentTypeCode, IShipmentPackageTypesBuilder> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Gets the ShipmentPackageTypesBuilder based on shipment type code.
        /// </summary>
        public IShipmentPackageTypesBuilder Get(ShipmentTypeCode shipmentTypeCode) =>
            lookup[shipmentTypeCode];
    }
}