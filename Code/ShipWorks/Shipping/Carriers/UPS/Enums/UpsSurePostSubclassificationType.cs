using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Possible values for SurePost Sub class
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsSurePostSubclassificationType
    {
        [Description("Irregular")]
        [ApiValue("IR")]
        Irregular = 0,

        [Description("Machineable")]
        [ApiValue("MA")]
        Machineable = 1
    }
}
