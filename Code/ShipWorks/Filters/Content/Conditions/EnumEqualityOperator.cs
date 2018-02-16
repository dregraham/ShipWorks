using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Provides enum equals\not\is in list\is not in list comparisons
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum EnumEqualityOperator
    {
        [Description("Equals")]
        Equals = 0,

        [Description("Does Not Equal")]
        NotEqual = 1,

        [Description("Is In List")]
        IsInList = 2,

        [Description("Is Not In List")]
        NotIsInList = 3
    }
}
