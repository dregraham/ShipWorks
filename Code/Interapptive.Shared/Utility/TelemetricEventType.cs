using System.ComponentModel;
using System.Reflection;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Event being timed
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
    public enum TelemetricEventType
    {
        [Description("GetLabel")]
        GetLabel = 0,

        [Description("CleanseAddress")]
        CleanseAddress = 1,

        [Description("GetRates")]
        GetRates = 2,

        [Description("DatabaseUpdate")]
        DatabaseUpdate = 3
    }
}
