using System;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Package version of the enum checkbox control
    /// </summary>
    [CLSCompliant(false)]
    public abstract class PackageTypePickerControl<T> : EnumCheckBoxControl<T> where T : struct, IConvertible
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PackageTypePickerControl()
        {
            Title = "Available Package Types";
            Description = "ShipWorks can be configured to only show the package types that are important to you. Simply select the package types below.";
        }
    }
}
