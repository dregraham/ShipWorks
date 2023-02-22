using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment
{
    /// <summary>
    /// Represents codes used by BuyDotCom confirmation "tracking" values. They use the numbers.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BuyDotComTrackingType
    {
        [Description("Ups")]
        Ups = 1,

        [Description("FedEx")]
        FedEx = 2,

        [Description("Usps")]
        Usps = 3,

        [Description("Other")]
        Other = 5,
        
        [Description("DHL")]
        DHL = 4,

        [Description("UPS MI")]
        UPSMI = 6,

        [Description("FedEx FedEx Ground® Economy")]
        FedExSmartPost = 7,

        [Description("DHL Global Mail")]
        DHLGlobalMail = 8
    }
}