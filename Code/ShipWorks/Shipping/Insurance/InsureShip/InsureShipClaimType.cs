using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum InsureShipClaimType
    {
        [Description("Damage")]
        [ApiValue("Damage")]
        Damage = 0,

        [Description("Lost")]
        [ApiValue("Lost")]
        Lost = 1,

        [Description("Missing")]
        [ApiValue("Missing")]
        Missing = 2
    }
}
