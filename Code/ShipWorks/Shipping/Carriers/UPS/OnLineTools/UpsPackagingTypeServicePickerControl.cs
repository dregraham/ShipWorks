using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// UPS implementation of the generic carrier package type picker control so it can be used in the designer
    /// </summary>
    [CLSCompliant(false)]
    public class UpsPackagingTypeServicePickerControl : EnumCheckBoxControl<UpsPackagingType>
    {
        /// <summary>
        /// Initializes the control with the available enums and the enums that have been excluded.
        /// </summary>
        /// <param name="availableEnums">All of the available enum values.</param>
        /// <param name="excludedEnums">The enum values that have been excluded and will be unchecked.</param>
        public void Initialize(IEnumerable<UpsPackagingType> availableEnums, IEnumerable<UpsPackagingType> excludedEnums)
        {
            Initialize(availableEnums, excludedEnums, "Available Packaging Types", "ShipWorks can be configured to only show the packaging types that are important to you. Simply select the packaging types below.");
        } 
    }
}