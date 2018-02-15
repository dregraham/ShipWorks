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
        ShippingProfileControlBase Create(ShipmentTypeCode shipmentType);
    }
}