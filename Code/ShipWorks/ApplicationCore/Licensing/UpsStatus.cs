using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsStatus
    {
        [ApiValue("0")]
        [Description("None")]
        None,

        [ApiValue("1")]
        [Description("Discount")]
        Discount = 1,

        [ApiValue("2")]
        [Description("Subsidized")]
        Subsidized = 2,

        [ApiValue("3")]
        [Description("Tier1")]
        Tier1 = 3,

        [ApiValue("4")]
        [Description("Tier2")]
        Tier2 = 4,

        [ApiValue("5")]
        [Description("Tier3")]
        Tier3 = 5
    }
}
