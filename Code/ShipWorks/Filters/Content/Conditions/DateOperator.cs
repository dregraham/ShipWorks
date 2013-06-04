using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DateOperator
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
        NotBetween = 7,

        [Description("Is Today")]
        Today = 8,

        [Description("Is Yesterday")]
        Yesterday = 9,

        [Description("Is in This")]
        This = 10,

        [Description("Is in Last")]
        Last = 11,

        [Description("Is Within the Last")]
        WithinTheLast = 12,
    }
}
