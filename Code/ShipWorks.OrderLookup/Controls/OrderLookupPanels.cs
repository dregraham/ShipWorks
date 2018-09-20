using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.OrderLookup.Controls
{
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum OrderLookupPanels
    {
        [ApiValue("From")]
        [Description("From")]
        From,

        [ApiValue("To")]
        [Description("To")]
        To,

        [ApiValue("LabelOptions")]
        [Description("Label Options")]
        LabelOptions
    }
}
