using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shared.System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Enum for date comparisons
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ValueCompareOperatorType
    {
        LessThan = 0,

        LessThanOrEqualTo = 1,

        Equal = 2,

        GreaterThanOrEqualTo = 3,

        GreaterThan = 4
    }
}
