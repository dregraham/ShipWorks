using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Actions.Tasks.Common
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum CleanupDatabaseType
    {
        [Description("Audit")]
        Audit = 0,

        [Description("Email")]
        Email = 1,

        [Description("Label")]
        Label = 2,

        [Description("Print Job")]
        Weekly = 3
    }
}
