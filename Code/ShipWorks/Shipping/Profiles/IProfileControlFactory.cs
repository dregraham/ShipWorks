namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Factory for a profile control
    /// </summary>
    public interface IProfileControlFactory
    {
        /// <summary>
        /// Creates a profile control
        /// </summary>
        ShippingProfileControlBase Create();

        /// <summary>
        /// Creates a profile control for the given shipment type
        /// </summary>
        ShippingProfileControlBase Create(ShipmentTypeCode shipmentType);
    }
}