using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Valid archive return types
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum OrderArchiveResult
    {
        [Description("Succeeded")]
        [ApiValue("Success")]
        Succeeded = 0,

        [Description("Failed")]
        [ApiValue("Failed")]
        Failed = 1,

        [Description("Cancelled")]
        [ApiValue("Cancelled")]
        Cancelled = 2
    }
}