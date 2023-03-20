using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
        [Description("UPS")]
        [ApiValue("UPS")]
        UPS,

        [Description("USPS")]
        [ApiValue("USPS")]
        USPS,

        [Description("FedEx")]
        [ApiValue("FedEx")]
        FedEx,

        [Description("Airborne")]
        [ApiValue("Airborne")]
        Airborne,

        [Description("OnTrac")]
        [ApiValue("OnTrac")]
        OnTrac,

        [Description("DHL")]
        [ApiValue("DHL")]
        DHL,

        [Description("NG")]
        [ApiValue("NG")]
        NG,

        [Description("LS")]
        [ApiValue("LS")]
        LS,

        [Description("UDS")]
        [ApiValue("UDS")]
        UDS,

        [Description("UPSMI")]
        [ApiValue("UPSMI")]
        UPSMI,

        [Description("FDX")]
        [ApiValue("FDX")]
        FDX,

        [Description("PILOT")]
        [ApiValue("PILOT")]
        PILOT,

        [Description("ESTES")]
        [ApiValue("ESTES")]
        ESTES,

        [Description("SAIA")]
        [ApiValue("SAIA")]
        SAIA,

        [Description("DHL Ecommerce - US")]
        [ApiValue("DHL Ecommerce - US")]
        DHL_Ecommerce_US,

        [Description("TForce Freight")]
        [ApiValue("TForce Freight")]
        TForceFreight
    }
}
