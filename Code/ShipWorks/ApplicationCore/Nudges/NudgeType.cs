using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Nudges
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum NudgeType
    {
        [Description("Upgrade ShipWorks")]
        [ApiValue("ShipWorksUpgrade")]
        ShipWorksUpgrade = 0
    }
}
