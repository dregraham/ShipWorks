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
        [ApiValue("Merchandise")]
        Merchandise = 0,

        [Description("Documents")]
        [ApiValue("documents")]
        Documents = 1,

        [Description("Gift")]
        [ApiValue("gift")]
        Gift = 2,

        [Description("Returned Goods")]
        [ApiValue("returned_goods")]
        ReturnedGoods = 3,

        [Description("Sample")]
        [ApiValue("sample")]
        Sample = 4
    }
}
