using System;
using System.Collections.Generic;
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

        [ApiValue("UPS")]
        UPS,

        [ApiValue("USPS")]
        USPS,

        [ApiValue("FedEx")]
        FedEx,

        [ApiValue("Airborne")]
        Airborne,

        [ApiValue("OnTrac")]
        OnTrac,

        [ApiValue("DHL")]
        DHL,

        [ApiValue("NG")]
        NG,

        [ApiValue("LS")]
        LS,

        [ApiValue("UDS")]
        UDS,

        [ApiValue("UPSMI")]
        UPSMI,

        [ApiValue("FDX")]
        FDX,

        [ApiValue("PILOT")]
        PILOT,

        [ApiValue("ESTES")]
        ESTES,

        [ApiValue("SAIA")]
        SAIA,

        [ApiValue("DHL Ecommerce - US")]
        DHL_Ecommerce_US,

        [ApiValue("TForce Freight")]
        TForceFreight
    }
}
