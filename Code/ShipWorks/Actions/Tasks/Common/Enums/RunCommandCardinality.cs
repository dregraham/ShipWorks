using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Actions.Tasks.Common.Enums
{
    /// <summary>
    /// Controls how the Run Command task uses its input to create separate commands
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum RunCommandCardinality
    {
        [Description("One Time")]
        OneTime = 0,

        [Description("Once for each {0} in the filter")]
        OncePerFilterEntry = 1
    }
}
