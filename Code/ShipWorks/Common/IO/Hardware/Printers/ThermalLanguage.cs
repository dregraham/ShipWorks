using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Thermal languages
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ThermalLanguage
    {
        [Hidden]
        [Description("Standard")]
        None = -1,

        [Description("EPL")]
        EPL = 0,

        [Description("ZPL")]
        ZPL = 1
    }
}
