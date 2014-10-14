using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Common.IO.Hardware.Printers
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
