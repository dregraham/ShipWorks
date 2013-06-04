using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Units for "within" date condition operators
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DateWithinUnit
    {
        [Description("Days")]
        Days = 0,

        [Description("Weeks")]
        Weeks = 1,

        [Description("Months")]
        Months = 2,

        [Description("Quarters")]
        Quarters = 3,

        [Description("Years")]
        Years = 4
    }
}
