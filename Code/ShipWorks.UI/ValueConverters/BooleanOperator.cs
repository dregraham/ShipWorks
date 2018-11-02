using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Operator to use for boolean comparisons
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum BooleanOperator
    {
        [Description("And")]
        And,

        [Description("Or")]
        Or
    }
}
