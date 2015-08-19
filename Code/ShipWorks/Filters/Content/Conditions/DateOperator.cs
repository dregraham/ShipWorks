using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Filters.Content.Conditions
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DateOperator
    {
        [SortOrder(0)]
        [Description("Equals")]
        Equal = 0,

        [SortOrder(1)]
        [Description("Does Not Equal")]
        NotEqual = 1,

        [SortOrder(2)]
        [Description("Is Greater Than")]
        GreaterThan = 2,

        [SortOrder(3)]
        [Description("Is Greater Than or Equal To")]
        GreaterThanOrEqual = 3,

        [SortOrder(4)]
        [Description("Is Less Than")]
        LessThan = 4,

        [SortOrder(5)]
        [Description("Is Less Than or Equal To")]
        LessThanOrEqual = 5,

        [SortOrder(6)]
        [Description("Is Between")]
        Between = 6,

        [SortOrder(7)]
        [Description("Is Not Between")]
        NotBetween = 7,

        [SortOrder(8)]
        [Description("Is Today")]
        Today = 8,

        [SortOrder(9)]
        [Description("Is Tomorrow")]
        Tomorrow = 13,

        [SortOrder(10)]
        [Description("Is Yesterday")]
        Yesterday = 9,

        [SortOrder(11)]
        [Description("Is in This")]
        This = 10,

        [SortOrder(12)]
        [Description("Is in Last")]
        Last = 11,

        [SortOrder(13)]
        [Description("Is Within the Last")]
        WithinTheLast = 12,

        [SortOrder(14)]
        [Description("Is in the Next")]
        Next = 14,
    }
}
