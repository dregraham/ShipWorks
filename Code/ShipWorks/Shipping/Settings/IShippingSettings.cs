using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages the global shipping settings instance
    /// </summary>
    public interface IShippingSettings
    {
        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        void InitializeForCurrentDatabase();

        /// <summary>
        /// Should shipments be auto created
        /// </summary>
        bool AutoCreateShipments { get; }

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

        /// <summary>
        /// Sets the default shipping provider to the given shipment type code.
        /// </summary>
        void SetDefaultProvider(ShipmentTypeCode shimentTypeCode);
		
        /// <summary>
        /// Save the current shipping settings
        /// </summary>
        void Save(ShippingSettingsEntity shippingSettings);
    }
}