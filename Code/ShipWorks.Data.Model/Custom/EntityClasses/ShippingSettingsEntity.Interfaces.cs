using System.Collections.Generic;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Custom shipping settings data
    /// </summary>
    public partial interface IShippingSettingsEntity
    {
        /// <summary>
        /// List of shipments types that have been activated to by visible if selected in the shipping window.  This list will be the same as
        /// the Configured list except in the case of upgrading from 2x where they would need to be visible, but maybe not been through configuration yet.
        /// </summary>
        IEnumerable<ShipmentTypeCode> ActivatedTypes { get; }

        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        IEnumerable<ShipmentTypeCode> ConfiguredTypes { get; }

        /// <summary>
        /// List of shipment types that the user has elected to have hidden from the ShipWorks UI for selection and configuration.  This list is independent
        /// of the Activated and Configured lists.
        /// </summary>
        IEnumerable<ShipmentTypeCode> ExcludedTypes { get; }

        /// <summary>
        /// List of shipment types that the user has elected to exclude when attempting to get the cheapest rate.
        /// </summary>
        IEnumerable<ShipmentTypeCode> BestRateExcludedTypes { get; }

        /// <summary>
        /// Strongly typed default shipment type code
        /// </summary>
        ShipmentTypeCode DefaultShipmentTypeCode { get; }
    }
}
