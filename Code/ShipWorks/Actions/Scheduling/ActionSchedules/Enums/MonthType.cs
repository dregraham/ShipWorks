using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum MonthType
    {
        [Description("January")]
        [XmlEnum("0")]
        January = 0,

        [Description("February")]
        [XmlEnum("1")]
        February = 1,

        [Description("March")]
        [XmlEnum("2")]
        March = 2,

        [Description("April")]
        [XmlEnum("3")]
        April = 3,

        [Description("May")]
        [XmlEnum("4")]
        May = 4,

        [Description("June")]
        [XmlEnum("5")]
        June = 5,

        [Description("July")]
        [XmlEnum("6")]
        July = 6,

        [Description("August")]
        [XmlEnum("7")]
        August = 7,

        [Description("September")]
        [XmlEnum("8")]
        September = 8,

        [Description("October")]
        [XmlEnum("9")]
        October = 9,

        [Description("November")]
        [XmlEnum("10")]
        November = 10,

        [Description("December")]
        [XmlEnum("11")]
        December = 11
    }
}
