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

        [Description("Email"), ApiValue("PurgeEmailOutbound")]
        Email = 2,

        [Description("Labels"), ApiValue("PurgeLabels")]
        Labels = 3,

        [Description("Print Jobs"), ApiValue("PurgePrintResult")]
        PrintJobs = 4,

        [Description("Abandoned Resources"), ApiValue("PurgeAbandonedResources")]
        AbandonedResources = 5,

        [Description("Orders"), ApiValue("PurgeOrders")]
        Orders = 6,

        [Description("Downloads"), ApiValue("PurgeDownloads")]
        Downloads = 7
    }
}
