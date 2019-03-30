using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Available Amazon SWA service types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonSWAServiceType
    {
        [Description("Fastest Possible")]
        [ApiValue("amazon_shipping_standard")]
        Fastest = 0,

        [Description("1 Day")]
        [ApiValue("1")]
        OneDay = 1,

        [Description("2 Days")]
        [ApiValue("2")]
        TwoDays = 2,

        [Description("3 Days")]
        [ApiValue("3")]
        ThreeDays = 3,

        [Description("4 Days")]
        [ApiValue("4")]
        FourDays = 4,

        [Description("5 Days")]
        [ApiValue("5")]
        FiveDays = 5,

        [Description("Lowest Price")]
        [ApiValue("99")]
        LowestPrice = 99,
    }
}
