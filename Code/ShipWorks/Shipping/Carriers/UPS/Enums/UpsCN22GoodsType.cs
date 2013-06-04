using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// UPS Goods Types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsCN22GoodsType
    {
        [Description("Gifts")]
        [ApiValueAttribute("1")]
        Gifts = 0,

        [Description("Documents")]
        [ApiValueAttribute("2")]
        Documents = 1,

        [Description("Commercial Sample")]
        [ApiValueAttribute("3")]
        CommercialSample = 2,

        [Description("Other")]
        [ApiValueAttribute("4")]
        Other = 3
    }
}
