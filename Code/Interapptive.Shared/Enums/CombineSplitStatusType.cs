using System.ComponentModel;
using System.Reflection;

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
        Combined = 1
    }
}
