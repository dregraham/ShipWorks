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
    /// UPS VendorCollectIDTypeCode response status codes found in the Shipping API. This was an addition in June 2021.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsCustomsRecipientTINType
    {
        [ApiValue("0356")] 
        [Description("IOSS")]
        IOSS,

        [ApiValue("0357")]
        [Description("VOEC")]
        VOEC,

        [ApiValue("0358")]
        [Description("HMRC")]
        HMRC
    }
}
