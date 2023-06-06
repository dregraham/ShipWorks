using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// This was originally created from Walmart's XSD. But, every time they added a carrier, we would have to come up with 
    /// a new release. This was moved when we generalized Carrier Type to a string, but wanted to make sure existing functionality
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum WalmartCarrierType
    {
        [ApiValue("UPS")]
        [Description("UPS")]
        UPS,

        [ApiValue("USPS")]
        [Description("USPS")]
        USPS,

        [ApiValue("FedEx")]
        [Description("FedEx")]
        FedEx,

        [ApiValue("Airborne")]
        [Description("Airborne")]
        Airborne,

        [ApiValue("OnTrac")]
        [Description("OnTrac")]
        OnTrac,

        [ApiValue("DHL")]
        [Description("DHL")]
        DHL,

        [ApiValue("NG")]
        [Description("NG")]
        NG,

        [ApiValue("LS")]
        [Description("LS")]
        LS,

        [ApiValue("UDS")]
        [Description("UDS")]
        UDS,

        [ApiValue("UPSMI")]
        [Description("UPSMI")]
        UPSMI,

        [ApiValue("FDX")]
        [Description("FDX")]
        FDX,

        [ApiValue("PILOT")]
        [Description("PILOT")]
        PILOT,

        [ApiValue("ESTES")]
        [Description("ESTES")]
        ESTES,

        [ApiValue("SAIA")]
        [Description("SAIA")]
        SAIA,

        [ApiValue("DHL Ecommerce - US")]
        [Description("DHL Ecommerce - US")]
        DHL_Ecommerce_US,

        [ApiValue("TForce Freight")]
        [Description("TForce Freight")]
        TForceFreight
    }
}
