using Interapptive.Shared.Collections;
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
        public int[] ActivatedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalActivated);
            }
            set
            {
                InternalActivated = ArrayUtility.FormatCommaSeparatedList(value);
            }
        }

        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        public int[] ConfiguredTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalConfigured);
            }
            set
            {
                InternalConfigured = ArrayUtility.FormatCommaSeparatedList(value);
            }
        }

        /// <summary>
        /// List of shipment types that the user has elected to have hidden from the ShipWorks UI for selection and configuration.  This list is independent
        /// of the Activated and Configured lists.
        /// </summary>
        public int[] ExcludedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalExcluded);
            }
            set
            {
                InternalExcluded = ArrayUtility.FormatCommaSeparatedList(value);
            }
        }

        /// <summary>
        /// List of shipment types that the user has elected to exclude when attempting to get the cheapest rate.
        /// </summary>
        public int[] BestRateExcludedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalBestRateExcludedShipmentTypes);
            }
            set
            {
                InternalBestRateExcludedShipmentTypes = ArrayUtility.FormatCommaSeparatedList(value);
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
    }
}
