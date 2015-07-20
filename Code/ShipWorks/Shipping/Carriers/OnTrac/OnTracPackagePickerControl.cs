using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Simple implementation of the generic carrier package picker control so it can be used in the designer
    /// </summary>
    [CLSCompliant(false)]
    public class OnTracPackagePickerControl : EnumCheckBoxControl<OnTracPackagingType>
    {
        /// <summary>
        /// Initializes the control with the available enums and the enums that have been excluded.
        /// </summary>
        /// <param name="availableEnums">All of the available enum values.</param>
        /// <param name="excludedEnums">The enum values that have been excluded and will be unchecked.</param>
        public void Initialize(IEnumerable<OnTracPackagingType> availableEnums, IEnumerable<OnTracPackagingType> excludedEnums)
        {
            Initialize(availableEnums, excludedEnums, "Available Package Types", "ShipWorks can be configured to only show the package types that are important to you. Simply select the packages types below.");
        }
    }
}