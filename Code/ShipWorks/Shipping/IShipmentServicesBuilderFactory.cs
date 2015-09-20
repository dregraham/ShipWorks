namespace ShipWorks.Shipping
{
    /// <summary>
    /// Factory for a ShipmentServicesBuilder
    /// </summary>
    public interface IShipmentServicesBuilderFactory
    {
        /// <summary>
        /// Gets the ShipmentServicesBuilder based on shipment type code.
        /// </summary>
        IShipmentServicesBuilder Get(ShipmentTypeCode shipmentTypeCode);
    }
}