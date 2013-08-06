using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum MonthlyCalendarType
    {
        [Description("Date of Month")]
        [XmlEnum("0")]
        Date = 0,

        [Description("Day of Week")]
        [XmlEnum("1")]
        Day = 1
    }
}