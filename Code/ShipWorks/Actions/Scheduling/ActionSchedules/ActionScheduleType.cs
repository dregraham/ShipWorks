using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionScheduleType
    {
        [Description("One Time")]
        OneTime = 0
    }
}
