namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages the global shipping settings instance
    /// </summary>
    public interface IShippingSettings
    {
        /// <summary>
        /// Marks the given ShipmentTypeCode as completely configured
        /// </summary>
        void MarkAsConfigured(ShipmentTypeCode shipmentTypeCode);
    }
}