namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Provide a prefetch path for a shipment
    /// </summary>
    public interface IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        PrefetchPathContainer GetPath();
    }
}
