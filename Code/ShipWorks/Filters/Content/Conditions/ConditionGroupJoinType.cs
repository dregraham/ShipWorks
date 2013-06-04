using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Specifies how two ConditionGroup results should be joined together.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ConditionGroupJoinType
    {
        [Description("And")]
        And = 0,

        [Description("Or")]
        Or = 1
    }
}
