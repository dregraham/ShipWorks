using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Predefined primary Reason Code for opening a UPS account
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPrimaryReason
    {
        [ApiValue("01")]
        [Description("Starting a new business")]
        NewBusiness = 0,

        [ApiValue("02")]
        [Description("Currently using another carrier")]
        OtherCarrier = 1,

        [ApiValue("06")]
        [Description("Occasional shipping needs")]
        OccasionalShippingNeeds = 2
    }
}
