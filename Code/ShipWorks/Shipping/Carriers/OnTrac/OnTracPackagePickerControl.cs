using System;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Simple implementation of the generic carrier package picker control so it can be used in the designer
    /// </summary>
    [CLSCompliant(false)]
    public class OnTracPackagePickerControl : EnumCheckBoxControl<OnTracPackagingType>
    {

    }
}