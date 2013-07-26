using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Actions.Tasks.Common
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PurgeDatabaseType
    {
        [Description("Audit Trail"), ApiValue("PurgeAudit")]
        Audit = 0,

        [Description("Download Records"), ApiValue("PurgeDownload")]
        Downloads = 1,

        [Description("Email"), ApiValue("PurgeEmail")]
        Email = 2,

        [Description("Labels"), ApiValue("PurgeLabel")]
        Labels = 3,

        [Description("Print Jobs"), ApiValue("PurgePrintResult")]
        PrintJobs = 4
    }
}
