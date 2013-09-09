using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Actions.Triggers
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UserInitiatedSelectionRequirement
    {
        [Description("Nothing needs to be selected")]
        None = 0,

        [Description("An order to be selected")]
        Orders = 1,

        [Description("A customer to be selected")]
        Customers = 2
    }
}
