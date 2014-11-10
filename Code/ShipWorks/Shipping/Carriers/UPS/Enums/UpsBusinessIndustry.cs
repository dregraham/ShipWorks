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
        [Description("Automotive")]
        [ApiValue("2000")]
        Automotive = 0,

        [Description("High Tech")]
        [ApiValue("3000")]
        HighTech = 1,

        [Description("Industrial Manufacturing & Distribution")]
        [ApiValue("4000")]
        IndustrialManufacturingAndDistribution = 2,

        [Description("Retail And Consumer Goods")]
        [ApiValue("5000")]
        RetailAndConsumerGoods = 3,

        [Description("Professional Services")]
        [ApiValue("6000")]
        ProfessionalServices = 4,

        [Description("Consumer Services")]
        [ApiValue("7000")]
        ConsumerServices = 5,

        [Description("Healthcare")]
        [ApiValue("8000")]
        Healthcare = 6,

        [Description("Government")]
        [ApiValue("9000")]
        Government = 7,

        [Description("Other")]
        [ApiValue("9900")]
        Other = 8
    }
}