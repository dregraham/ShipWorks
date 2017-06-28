using System.Reflection;
using System.ComponentModel;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Enum representing an order's combined, split, or none status
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum CombineSplitStatusType
    {
        [Description("None")]
        None = 0,

        [Description("Combined")]
        Combined = 1,

        [Description("Split")]
        Split = 2
    }
}
