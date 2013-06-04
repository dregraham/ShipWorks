using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Thermal printer types supported by ShipWorks
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ThermalLabelType
    {
        [Description("Eltron (EPL)")]
        EPL = 0,

        [Description("Zebra (ZPL)")]
        ZPL = 1
    }
}
