using Interapptive.Shared.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// ShipEngine Contents - This is used when sending customs content information to ShipEngine.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipEngineContentsType
    {
        [Description("Gift")]
        [ApiValue("gift")]
        Gift = 0,

        [Description("Merchandise")]
        [ApiValue("Merchandise")]
        Merchandise = 1,

        [Description("Returned Goods")]
        [ApiValue("returned_goods")]
        ReturnedGoods = 2,

        [Description("Documents")]
        [ApiValue("documents")]
        Documents = 3,

        [Description("Sample")]
        [ApiValue("sample")]
        Sample = 4
    }
}
