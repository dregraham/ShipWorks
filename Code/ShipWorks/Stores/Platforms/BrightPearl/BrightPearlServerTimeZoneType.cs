using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Brightpearl
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BrightpearlServerTimeZoneType
    {
        [Description("GMT or CET")] 
        [ApiValue("eu1")]
        Eu1,

        [Description("PST or MST")]
        [ApiValue("usw")]
        Usw,

        [Description("EST or CST")]
        [ApiValue("use")]
        Use
    }
}
