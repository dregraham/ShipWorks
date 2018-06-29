using System.ComponentModel;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Event being timed
    /// </summary>
    public enum TelemetricEventType
    {
        [Description("GetLabel")]
        GetLabel = 0,

        [Description("CleanseAddress")]
        CleanseAddress = 1,

        [Description("GetRates")]
        GetRates = 2
    }
}
