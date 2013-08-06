using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Actions
{
    /// <summary>
    /// An enumeration for specifying the type of action queue entry.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionQueueType
    {
        [Description("UI")]
        UserInterface = 0,

        [Description("Scheduled")]
        Scheduled = 1
    }
}
