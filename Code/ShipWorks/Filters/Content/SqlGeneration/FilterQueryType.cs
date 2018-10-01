using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// The type of filter query, Initial, Update, or entity exists.
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public enum FilterQueryType
    {
        [Description("Initial")]
        Initial = 0,

        [Description("Update")]
        Update = 1,

        [Description("Exists")]
        Exists = 2
    }
}
