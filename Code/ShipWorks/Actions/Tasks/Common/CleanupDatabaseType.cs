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

        [Description("Downloads")]
        Downloads = 1,

        [Description("Email")]
        Email = 2,

        [Description("Labels")]
        Labels = 3,

        [Description("Print Jobs")]
        PrintJobs = 4
    }
}
