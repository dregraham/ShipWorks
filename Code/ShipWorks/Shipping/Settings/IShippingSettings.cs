using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;

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

        /// <summary>
        /// Fetch the current shipping settings
        /// </summary>
        ShippingSettingsEntity Fetch();
		
        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        IEnumerable<ShipmentTypeCode> GetConfiguredTypes();
    }
}