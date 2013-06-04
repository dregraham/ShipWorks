using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Specifies how the results of all conditions in a group are joined together to form a single result.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ConditionJoinType
    {
        [Description("All")]
        All = 0,

        [Description("Any")]
        Any = 1,

        [Description("None")]
        None = 2
    }
}
