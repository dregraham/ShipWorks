using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Provides simple equals\not equals comparison
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum EqualityOperator
    {
        [Description("Equals")]
        Equals = 0,

        [Description("Does Not Equal")]
        NotEqual = 1,
    }
}
