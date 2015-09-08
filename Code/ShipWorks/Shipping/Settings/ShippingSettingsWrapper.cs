using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages the global shipping settings instance
    /// </summary>
    /// <remarks>
    /// Wraps the static ShippingSettings so that static dependencies can be broken
    /// </remarks>
    public class ShippingSettingsWrapper : IShippingSettings
    {
        /// <summary>
        /// Marks the given ShipmentTypeCode as completely configured
        /// </summary>
        public void MarkAsConfigured(ShipmentTypeCode shipmentTypeCode) =>
            ShippingSettings.MarkAsConfigured(shipmentTypeCode);
        
        /// <summary>
        /// Fetch the current shipping settings
        /// </summary>
        public ShippingSettingsEntity Fetch() => ShippingSettings.Fetch();
    }
}