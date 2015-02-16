using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Enum for Stamps resellers 
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UspsResellerType
    {
        [Description("None")]
        None = 0,

        [Description("Express1")]
        Express1 = 1,

        [Description("USPS")]
        StampsExpedited = 2
    }
}
