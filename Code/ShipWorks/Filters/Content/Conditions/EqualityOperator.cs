using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Provides simple equals\not equals comparison
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EqualityOperator
    {
        [Description("Equals")]
        Equals = 0,

        [Description("Does Not Equal")]
        NotEqual = 1,
    }
}
