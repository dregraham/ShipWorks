using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Types of shipments we support.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipmentTypeCode
    {
        [Description("UPS")]
        UpsOnLineTools = 0,

        [Description("UPS (WorldShip)")]
        UpsWorldShip = 1,

        [Description("USPS (Endicia)")]
        Endicia = 2,

        [Description("USPS (Stamps.com)")]
        Stamps = 3,

        [Description("USPS (w/o Postage)")]
        PostalWebTools = 4,

        [Description("USPS (Express1)")]
        PostalExpress1 = 9,

        [Description("FedEx")]
        FedEx = 6,
        
        [Description("OnTrac")]
        OnTrac = 11,

        [Description("i-parcel")]
        iParcel = 12,

        [Description("Other")]
        Other = 5,

        [Description("EquaShip")]
        EquaShip = 10,

        [Description("None")]
        None = 99
    }
}
