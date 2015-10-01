namespace ShipWorks.Shipping.Services.Builders
{
    /// <summary>
    /// Factory for a ShipmentServicesBuilder
    /// </summary>
    public interface IShipmentPackageBuilderFactory
    {
        /// <summary>
        /// Gets the ShipmentServicesBuilder based on shipment type code.
        /// </summary>
        IShipmentPackageTypesBuilder Get(ShipmentTypeCode shipmentTypeCode);
    }
}