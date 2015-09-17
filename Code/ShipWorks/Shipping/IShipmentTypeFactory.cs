namespace ShipWorks.Shipping
{
    /// <summary>
    /// Retrieve a shipment type based on its code
    /// </summary>
    /// <remarks>
    /// It's easier to test when the factory is a real type rather than
    /// relying on using Autofac's IIndex implementation.</remarks>
    public interface IShipmentTypeFactory
    {
        /// <summary>
        /// Get the shipment type based on its code
        /// </summary>
        ShipmentType GetType(ShipmentTypeCode shipmentTypeCode);
    }
}
