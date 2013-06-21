using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum WeekOfMonthType
    {
        [Description("First")]
        [XmlEnum("0")]
        First = 0,

        [Description("Second")]
        [XmlEnum("1")]
        Second = 1,

        [Description("Third")]
        [XmlEnum("2")]
        Third = 2,

        [Description("Fourth")]
        [XmlEnum("3")]
        Fourth = 3
    }
}
