using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Settings;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Custom shipping settings data
    /// </summary>
    public partial class ReadOnlyShippingSettingsEntity
    {
        /// <summary>
        /// List of shipments types that have been activated to by visible if selected in the shipping window.  This list will be the same as
        /// the Configured list except in the case of upgrading from 2x where they would need to be visible, but maybe not been through configuration yet.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ActivatedTypes { get; private set; }

        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ConfiguredTypes { get; private set; }

        /// <summary>
        /// List of shipment types that the user has elected to have hidden from the ShipWorks UI for selection and configuration.  This list is independent
        /// of the Activated and Configured lists.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ExcludedTypes { get; private set; }

        /// <summary>
        /// List of shipment types that the user has elected to exclude when attempting to get the cheapest rate.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> BestRateExcludedTypes { get; private set; }

        /// <summary>
        /// Strongly typed default shipment type code
        /// </summary>
        public ShipmentTypeCode DefaultShipmentTypeCode { get; private set; }

        /// <summary>
        /// Get the shipment date cutoff for the given shipment type
        /// </summary>
        public ShipmentDateCutoff GetShipmentDateCutoff(ShipmentTypeCode shipmentType)
        {
            throw new NotImplementedException("This is just a stub");
        }

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomShippingSettingsData(IShippingSettingsEntity source)
        {
            ActivatedTypes = source.ActivatedTypes.ToReadOnly();
            ConfiguredTypes = source.ConfiguredTypes.ToReadOnly();
            ExcludedTypes = source.ExcludedTypes.ToReadOnly();
            BestRateExcludedTypes = source.BestRateExcludedTypes.ToReadOnly();
            DefaultShipmentTypeCode = source.DefaultShipmentTypeCode;
        }
    }
}
