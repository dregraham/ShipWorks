using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Operators that can be applied to values of numeric conditions
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum NumericOperator
    {
        [Description("Equals")]
        Equal = 0,

        [Description("Does Not Equal")]
        NotEqual = 1,

        [Description("Is Greater Than")]
        GreaterThan = 2,

        [Description("Is Greater Than or Equal To")]
        GreaterThanOrEqual = 3,

        [Description("Is Less Than")]
        LessThan = 4,

        [Description("Is Less Than or Equal To")]
        LessThanOrEqual = 5,

        [Description("Is Between")]
        Between = 6,

        [Description("Is Not Between")]
        NotBetween = 7
    }
}
