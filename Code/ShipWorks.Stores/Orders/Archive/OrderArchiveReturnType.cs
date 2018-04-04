using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Valid archive return types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OrderArchiveReturnType
    {
        [Description("Success")]
        Success = 0,

        [Description("Failed")]
        Fail = 1,

        [Description("Cancelled")]
        Cancel = 2
    }
}