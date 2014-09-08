using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.ApplicationCore.Nudges
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum NudgeTypes
    {
        [Description("Upgrade ShipWorks")]
        ShipWorksUpgrade = 0
    }
}
