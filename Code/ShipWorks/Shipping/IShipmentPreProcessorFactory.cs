namespace ShipWorks.Shipping
{
    /// <summary>
    /// Create a shipment preprocessor factory
    /// </summary>
    public interface IShipmentPreProcessorFactory
    {
        /// <summary>
        /// Create a shipment preprocessor for the given shipment type
        /// </summary>
        IShipmentPreProcessor Create(ShipmentTypeCode shipmentTypeCode);
    }
}
