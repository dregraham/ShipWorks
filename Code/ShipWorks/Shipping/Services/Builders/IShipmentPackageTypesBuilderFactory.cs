namespace ShipWorks.Shipping.Services.Builders
{
    /// <summary>
    /// Factory for a getting an IShipmentPackageTypesBuilder
    /// </summary>
    public interface IShipmentPackageTypesBuilderFactory
    {
        /// <summary>
        /// Gets the IShipmentPackageTypesBuilder based on shipment type code.
        /// </summary>
        IShipmentPackageTypesBuilder Get(ShipmentTypeCode shipmentTypeCode);
    }
}