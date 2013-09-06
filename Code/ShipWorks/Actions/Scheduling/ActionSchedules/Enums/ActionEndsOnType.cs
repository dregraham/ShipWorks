using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ActionEndsOnType
    {
        [Description("Never")]
        [XmlEnum("0")]
        Never = 0,

        [Description("SpecificDateTime")]
        [XmlEnum("1")]
        SpecificDateTime = 1
    }
}
