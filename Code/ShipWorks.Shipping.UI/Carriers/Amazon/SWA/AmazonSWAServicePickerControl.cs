using System;
using Interapptive.Shared.Enums;
using ShipWorks.Shipping.Carriers.Amazon.SWA;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon.SWA
{
    /// <summary>
    /// Simple implementation of the generic carrier service picker control so it can be used in the designer
    /// </summary>
    [CLSCompliant(false)]
    public class AmazonSWAServicePickerControl : ServicePickerControl<AmazonSWAServiceType>
    {
    }
}