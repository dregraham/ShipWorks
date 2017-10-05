using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Settings;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ShippingSettingsEntity
    /// </summary>
    public partial class ShippingSettingsEntity
    {
        /// <summary>
        /// List of shipments types that have been activated to by visible if selected in the shipping window.  This list will be the same as
        /// the Configured list except in the case of upgrading from 2x where they would need to be visible, but maybe not been through configuration yet.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ActivatedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalActivated).Select(x => (ShipmentTypeCode) x);
            }
            set
            {
                InternalActivated = ArrayUtility.FormatCommaSeparatedList(value.Cast<int>().ToArray());
            }
        }

        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ConfiguredTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalConfigured).Select(x => (ShipmentTypeCode) x);
            }
            set
            {
                InternalConfigured = ArrayUtility.FormatCommaSeparatedList(value.Cast<int>().ToArray());
            }
        }

        /// <summary>
        /// List of shipment types that the user has elected to have hidden from the ShipWorks UI for selection and configuration.  This list is independent
        /// of the Activated and Configured lists.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ExcludedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalExcluded).Select(x => (ShipmentTypeCode) x);
            }
            set
            {
                InternalExcluded = ArrayUtility.FormatCommaSeparatedList(value.Cast<int>().ToArray());
            }
        }

        /// <summary>
        /// List of shipment types that the user has elected to exclude when attempting to get the cheapest rate.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> BestRateExcludedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalBestRateExcludedShipmentTypes).Select(x => (ShipmentTypeCode) x);
            }
            set
            {
                InternalBestRateExcludedShipmentTypes = ArrayUtility.FormatCommaSeparatedList(value.Cast<int>().ToArray());
            }
        }

        /// <summary>
        /// Strongly typed default shipment type code
        /// </summary>
        public ShipmentTypeCode DefaultShipmentTypeCode
        {
            get { return (ShipmentTypeCode) DefaultType; }
            set { DefaultType = (int) value; }
        }

        /// <summary>
        /// Get the shipment date cutoff for the given shipment type
        /// </summary>
        public ShipmentDateCutoff GetShipmentDateCutoff(ShipmentTypeCode shipmentType)
        {
            throw new NotImplementedException("This is just a stub");
        }

        /// <summary>
        /// Set the shipment date cutoff for the given shipment type
        /// </summary>
        public void SetShipmentDateCutoff(ShipmentTypeCode shipmentType, ShipmentDateCutoff cutoff)
        {
            throw new NotImplementedException("This is just a stub");
        }

        ///// <summary>
        ///// List of shipments types that have been activated to by visible if selected in the shipping window.  This list will be the same as
        ///// the Configured list except in the case of upgrading from 2x where they would need to be visible, but maybe not been through configuration yet.
        ///// </summary>
        //IEnumerable<ShipmentTypeCode> IShippingSettingsEntity.ActivatedTypes =>
        //    ActivatedTypes.Select(x => (ShipmentTypeCode) x);

        ///// <summary>
        ///// The list of shipment types that have been fully configured for use within ShipWorks
        ///// </summary>
        //IEnumerable<ShipmentTypeCode> IShippingSettingsEntity.ConfiguredTypes =>
        //    ConfiguredTypes.Select(x => (ShipmentTypeCode) x);

        ///// <summary>
        ///// List of shipment types that the user has elected to have hidden from the ShipWorks UI for selection and configuration.  This list is independent
        ///// of the Activated and Configured lists.
        ///// </summary>
        //IEnumerable<ShipmentTypeCode> IShippingSettingsEntity.ExcludedTypes =>
        //    ExcludedTypes.Select(x => (ShipmentTypeCode) x);

        ///// <summary>
        ///// List of shipment types that the user has elected to exclude when attempting to get the cheapest rate.
        ///// </summary>
        //IEnumerable<ShipmentTypeCode> IShippingSettingsEntity.BestRateExcludedTypes =>
        //    BestRateExcludedTypes.Select(x => (ShipmentTypeCode) x);
    }
}
