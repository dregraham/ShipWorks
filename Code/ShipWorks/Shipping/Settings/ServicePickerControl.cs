using System;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Service type version of the enum checkbox control
    /// </summary>
    [CLSCompliant(false)]
    public abstract class ServicePickerControl<T> : EnumCheckBoxControl<T> where T : struct, IConvertible
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServicePickerControl()
        {
            Title = "Available Services";
            Description = "ShipWorks can be configured to only show the service types that are important to you. Simply select the services below.";
        }
    }
}
