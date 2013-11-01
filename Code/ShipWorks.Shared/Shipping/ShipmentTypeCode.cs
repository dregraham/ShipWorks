using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

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

        [CallbackDescription("ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Express1EndiciaShipmentType", "ShipmentTypeName")]
        Express1Endicia = 9,

        [CallbackDescription("ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.Express1StampsShipmentType", "ShipmentTypeName")]
        Express1Stamps = 13,

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

        [Description("Best rate")]
        BestRate = 14,

        [Description("None")]
        None = 99
    }
}
