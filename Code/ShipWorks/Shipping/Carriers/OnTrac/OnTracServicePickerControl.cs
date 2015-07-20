using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Simple implementation of the generic carrier service picker control so it can be used in the designer
    /// </summary>
    [CLSCompliant(false)]
    public class OnTracServicePickerControl : EnumCheckBoxControl<OnTracServiceType>
    {
        /// <summary>
        /// Initializes the control with the available enums and the enums that have been excluded.
        /// </summary>
        /// <param name="availableEnums">All of the available enum values.</param>
        /// <param name="excludedEnums">The enum values that have been excluded and will be unchecked.</param>
        public void Initialize(IEnumerable<OnTracServiceType> availableEnums, IEnumerable<OnTracServiceType> excludedEnums)
        {
            Initialize(availableEnums, excludedEnums, "Available Services", "ShipWorks can be configured to only show the service types that are important to you. Simply select the services below.");
        }
    }
}