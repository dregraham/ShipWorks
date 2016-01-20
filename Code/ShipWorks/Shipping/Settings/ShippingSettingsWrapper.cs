using System.Collections.Generic;
using System.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages the global shipping settings instance
    /// </summary>
    /// <remarks>
    /// Wraps the static ShippingSettings so that static dependencies can be broken
    /// </remarks>
    public class ShippingSettingsWrapper : IShippingSettings, IInitializeForCurrentDatabase, ICheckForChangesNeeded
    {
        /// <summary>
        /// Should shipments be auto created
        /// </summary>
        public bool AutoCreateShipments => Fetch().AutoCreateShipments;

        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        public IEnumerable<ShipmentTypeCode> GetConfiguredTypes()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            return settings.ConfiguredTypes.Cast<ShipmentTypeCode>();
        }

        /// <summary>
        /// Marks the given ShipmentTypeCode as completely configured
        /// </summary>
        public void MarkAsConfigured(ShipmentTypeCode shipmentTypeCode) =>
            ShippingSettings.MarkAsConfigured(shipmentTypeCode);

        /// <summary>
        /// Fetch the current shipping settings
        /// </summary>
        public ShippingSettingsEntity Fetch() => ShippingSettings.Fetch();

        /// <summary>
        /// Check the database for the latest SystemData
        /// </summary>
        public void CheckForChangesNeeded() => ShippingSettings.CheckForChangesNeeded();

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) =>
            ShippingSettings.InitializeForCurrentDatabase();

        /// <summary>
        /// Save the current shipping settings
        /// </summary>
        public void Save(ShippingSettingsEntity shippingSettings) => ShippingSettings.Save(shippingSettings);
    }
}