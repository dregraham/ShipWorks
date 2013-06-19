using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionScheduleType
    {
        [Description("One Time")]
        OneTime = 0,

        [Description("Hourly")]
        Hourly = 1,

        [Description("Daily")]
        Daily = 2
    }
}
