using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionScheduleType
    {
        [Description("One Time")]
        [XmlEnum("0")]
        OneTime = 0,

        [Description("Hourly")]
        [XmlEnum("1")]
        Hourly = 1,

        [Description("Daily")]
        [XmlEnum("2")]
        Daily = 2,

        [Description("Weekly")]
        [XmlEnum("3")]
        Weekly = 3,

        [Description("Monthly")]
        [XmlEnum("4")]
        Monthly = 4
    }
}
