using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        public IEnumerable<ShipmentTypeCode> GetConfiguredTypes()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            return settings.ConfiguredTypes.Cast<ShipmentTypeCode>();
        }

        /// <summary>
        /// Sets the default shipping provider to the given shipment type code.
        /// </summary>
        public void SetDefaultProvider(ShipmentTypeCode shipmentTypeCode)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            settings.DefaultType = (int) shipmentTypeCode;

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Marks the given ShipmentTypeCode as completely configured
        /// </summary>
        public void MarkAsConfigured(ShipmentTypeCode shipmentTypeCode)
        {
            ShippingSettings.MarkAsConfigured(shipmentTypeCode);
        }

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public void InitializeForCurrentDatabase()
        {
            ShippingSettings.InitializeForCurrentDatabase();
        }
    }
}