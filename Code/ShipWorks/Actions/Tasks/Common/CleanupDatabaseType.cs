using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Actions.Tasks.Common
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum CleanupDatabaseType
    {
        [Description("Audit"), ApiValue("AuditCleanup")]
        Audit = 0,

        [Description("Downloads"), ApiValue("Download")]
        Downloads = 1,

        [Description("Email"), ApiValue("EmailCleanup")]
        Email = 2,

        [Description("Labels"), ApiValue("LabelCleanup")]
        Labels = 3,

        [Description("Print Jobs"), ApiValue("PrintResultCleanup")]
        PrintJobs = 4
    }
}
