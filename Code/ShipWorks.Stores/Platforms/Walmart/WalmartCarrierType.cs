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
        [Description("UPS")]
        UPS,

        [Description("USPS")]
        [ApiValue("USPS")]
        [Description("USPS")]
        USPS,

        [Description("FedEx")]
        [ApiValue("FedEx")]
        [Description("FedEx")]
        FedEx,

        [Description("Airborne")]
        [ApiValue("Airborne")]
        [Description("Airborne")]
        Airborne,

        [Description("OnTrac")]
        [ApiValue("OnTrac")]
        [Description("OnTrac")]
        OnTrac,

        [Description("DHL")]
        [ApiValue("DHL")]
        [Description("DHL")]
        DHL,

        [Description("NG")]
        [ApiValue("NG")]
        [Description("NG")]
        NG,

        [Description("LS")]
        [ApiValue("LS")]
        [Description("LS")]
        LS,

        [Description("UDS")]
        [ApiValue("UDS")]
        [Description("UDS")]
        UDS,

        [Description("UPSMI")]
        [ApiValue("UPSMI")]
        [Description("UPSMI")]
        UPSMI,

        [Description("FDX")]
        [ApiValue("FDX")]
        [Description("FDX")]
        FDX,

        [Description("PILOT")]
        [ApiValue("PILOT")]
        [Description("PILOT")]
        PILOT,

        [Description("ESTES")]
        [ApiValue("ESTES")]
        [Description("ESTES")]
        ESTES,

        [Description("SAIA")]
        [ApiValue("SAIA")]
        [Description("SAIA")]
        SAIA,

        [Description("DHL Ecommerce - US")]
        [ApiValue("DHL Ecommerce - US")]
        [Description("DHL Ecommerce - US")]
        DHL_Ecommerce_US,

        [Description("TForce Freight")]
        [ApiValue("TForce Freight")]
        [Description("TForce Freight")]
        TForceFreight
    }
}
