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
    /// <remarks>
    /// The numeric values of the enum values correspond to the numeric values of the enum values
    /// of InternationalOptions.ContentsEnum
    /// </remarks>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipEngineContentsType
    {
        [Description("Merchandise")]
        [ApiValue("merchandise")]
        Merchandise = 1,

        [Description("Documents")]
        [ApiValue("documents")]
        Documents = 2,

        [Description("Gift")]
        [ApiValue("gift")]
        Gift = 3,

        [Description("Returned Goods")]
        [ApiValue("returned_goods")]
        ReturnedGoods = 4,

        [Description("Sample")]
        [ApiValue("sample")]
        Sample = 5
    }
}
