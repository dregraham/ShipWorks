using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// UPS Business Industries
    /// 2000 = Automotive
    /// 3000 = High Tech
    /// 4000 = Industrial Manufacturing & Distribution
    /// 5000 = Retail And Consumer Goods
    /// 6000 = Professional Services
    /// 7000 = Consumer Services
    /// 8000 = Healthcare
    /// 9000 = Government
    /// 9900 = Other
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsBusinessIndustry
    {
        [Description("Select an Industry")]
        [ApiValue("0")]
        Unselected = 0,

        [Description("Automotive")]
        [ApiValue("2000")]
        Automotive = 2000,

        [Description("High Tech")]
        [ApiValue("3000")]
        HighTech = 3000,

        [Description("Industrial Manufacturing & Distribution")]
        [ApiValue("4000")]
        IndustrialManufacturingAndDistribution = 4000,

        [Description("Retail And Consumer Goods")]
        [ApiValue("5000")]
        RetailAndConsumerGoods = 5000,

        [Description("Professional Services")]
        [ApiValue("6000")]
        ProfessionalServices = 6000,

        [Description("Consumer Services")]
        [ApiValue("7000")]
        ConsumerServices = 7000,

        [Description("Healthcare")]
        [ApiValue("8000")]
        Healthcare = 8000,

        [Description("Government")]
        [ApiValue("9000")]
        Government = 9000,

        [Description("Other")]
        [ApiValue("9900")]
        Other = 9900
    }
}