namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Factory for creating shipping profile application strategies
    /// </summary>
    public interface IShippingProfileApplicationStrategyFactory
    {
        /// <summary>
        /// Create a profile application strategy for the given shipment type
        /// </summary>
        IShippingProfileApplicationStrategy Create(ShipmentTypeCode? shipmentType);
    }
}