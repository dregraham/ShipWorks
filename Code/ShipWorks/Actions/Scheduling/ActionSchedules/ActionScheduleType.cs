using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionScheduleType
    {
        [Description("One Time")]
        [XmlEnumAttribute("0")]
        OneTime = 0,

        [Description("Hourly")]
        [XmlEnumAttribute("1")]
        Hourly = 1,

        [Description("Daily")]
        [XmlEnumAttribute("2")]
        Daily = 2,

        [Description("Weekly")]
        [XmlEnumAttribute("3")]
        Weekly = 3,

        [Description("Monthly")]
        [XmlEnumAttribute("4")]
        Monthly = 4
    }
}
