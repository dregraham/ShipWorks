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
        [ShipmentTypeIconAttribute("ups")]
        UpsOnLineTools = 0,

        [Description("UPS (WorldShip)")]
        [ShipmentTypeIconAttribute("ups")]
        UpsWorldShip = 1,

        [Description("USPS (Endicia)")]
        [ShipmentTypeIconAttribute("endicia")]
        Endicia = 2,

        [Description("USPS (w/o Postage)")]
        [ShipmentTypeIconAttribute("usps")]
        PostalWebTools = 4,

        [CallbackDescription("ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Express1EndiciaShipmentType", "ShipmentTypeName")]
        [ShipmentTypeIconAttribute("express1")]
        Express1Endicia = 9,

        [CallbackDescription("ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.Express1StampsShipmentType", "ShipmentTypeName")]
        [ShipmentTypeIconAttribute("express1")]
        Express1Stamps = 13,

        [Description("FedEx")]
        [ShipmentTypeIconAttribute("fedex")]
        FedEx = 6,
        
        [Description("OnTrac")]
        [ShipmentTypeIconAttribute("ontrac")]
        OnTrac = 11,

        [Description("i-parcel")]
        [ShipmentTypeIconAttribute("iparcel")]
        iParcel = 12,

        [Description("Other")]
        [ShipmentTypeIconAttribute("other")]
        Other = 5,

        [Description("Best Rate")]
        [ShipmentTypeIconAttribute("bestrate")]
        BestRate = 14,

        [Description("USPS")]
        [ShipmentTypeIconAttribute("usps")]
        Usps = 15,

        [Description("None")]
        None = 99
    }
}
