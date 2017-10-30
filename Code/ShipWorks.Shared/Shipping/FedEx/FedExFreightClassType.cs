using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Enum representing FedExFreightClass
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExFreightClassType
    {
        /// <summary>
        /// None selected
        /// </summary>
        [Description("None")]
        [ApiValue("")]
        None = 0,

        [Description("Class 050")]
        [ApiValue("CLASS_050")]
        CLASS_050 = 2,

        [Description("Class 055")]
        [ApiValue("CLASS_055")]
        CLASS_055 = 3,

        [Description("Class 060")]
        [ApiValue("CLASS_060")]
        CLASS_060 = 4,

        [Description("Class 065")]
        [ApiValue("CLASS_065")]
        CLASS_065 = 5,

        [Description("Class 070")]
        [ApiValue("CLASS_070")]
        CLASS_070 = 6,

        [Description("Class 077_5")]
        [ApiValue("CLASS_077_5")]
        CLASS_077_5 = 7,

        [Description("CLASS_085")]
        [ApiValue("Class 085")]
        CLASS_085 = 8,

        [Description("Class 092_5")]
        [ApiValue("CLASS_092_5")]
        CLASS_092_5 = 9,

        [Description("Class 100")]
        [ApiValue("CLASS_100")]
        CLASS_100 = 10,

        [Description("Class 110")]
        [ApiValue("CLASS_110")]
        CLASS_110 = 11,

        [Description("Class 125")]
        [ApiValue("CLASS_125")]
        CLASS_125 = 12,

        [Description("Class 150")]
        [ApiValue("CLASS_150")]
        CLASS_150 = 13,

        [Description("Class 175")]
        [ApiValue("CLASS_175")]
        CLASS_175 = 14,

        [Description("Class 200")]
        [ApiValue("CLASS_200")]
        CLASS_200 = 15,

        [Description("Class 250")]
        [ApiValue("CLASS_250")]
        CLASS_250 = 16,

        [Description("Class 300")]
        [ApiValue("CLASS_300")]
        CLASS_300 = 17,

        [Description("Class 400")]
        [ApiValue("CLASS_400")]
        CLASS_400 = 18,

        [Description("CLASS_500")]
        [ApiValue("Class 500")]
        CLASS_500 = 19,
    }
}
