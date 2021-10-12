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
    /// UPS ConsigneeType response status codes found in the Shipping API. This was an addition in June 2021.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsCustomsRecipientType
    {
        [ApiValue("01")] 
        [Description("Business")]
        Business,

        [ApiValue("02")]
        [Description("Consumer")]
        Consumer,

        [ApiValue("NA")]
        [Description("Not Applicable")]
        NA
    }
}
