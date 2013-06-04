using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Standard operators for strings
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum StringOperator
    {
        [Description("Equals")]
        Equals = 0,

        [Description("Does Not Equal")]
        NotEqual = 1,

        [Description("Contains")]
        Contains = 2,

        [Description("Does Not Contain")]
        NotContains = 3,

        [Description("Begins With")]
        BeginsWith = 4,

        [Description("Ends With")]
        EndsWith = 5,

        [Description("Matches Regex")]
        Matches = 6,

        [Description("Is Blank")]
        IsEmpty = 7
    }
}
