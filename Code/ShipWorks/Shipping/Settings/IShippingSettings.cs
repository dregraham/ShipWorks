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
        /// Determine what the initial shipment type for the given order should be, given the shipping settings rules
        /// </summary>
        ShipmentType InitialShipmentType(ShipmentEntity shipment);
    }
}