using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Valid archive return types
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum OrderArchiveResult
    {
        [Description("Success")]
        Success = 0,

        [Description("Failed")]
        Fail = 1,

        [Description("Cancelled")]
        Cancel = 2
    }
}