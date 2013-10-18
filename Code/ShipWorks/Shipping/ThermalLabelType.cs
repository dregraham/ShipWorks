using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Thermal languages
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ThermalLanguage
    {
        [Description("Standard")]
        None = -1,

        [Description("EPL")]
        EPL = 0,

        [Description("ZPL")]
        ZPL = 1
    }
}
