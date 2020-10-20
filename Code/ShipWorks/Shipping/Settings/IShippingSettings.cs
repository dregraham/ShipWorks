using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

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
        /// Fetch the current shipping settings
        /// </summary>
        IShippingSettingsEntity FetchReadOnly();

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

        /// <summary>
        /// Notify the shipping settings class that it should check for changes
        /// </summary>
        void CheckForChangesNeeded();

        /// <summary>
        /// Is the given shipment type configured
        /// </summary>
        bool IsConfigured(ShipmentTypeCode shipmentType);

        /// <summary>
        /// Mark the given shipment type as enabled in the Shipping Settings UI
        /// </summary>
        void MarkAsEnabled(ShipmentTypeCode shipmentType);
    }
}