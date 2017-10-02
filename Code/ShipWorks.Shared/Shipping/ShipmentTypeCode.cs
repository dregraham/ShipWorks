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
        [ShipmentTypeIcon("ups")]
        UpsOnLineTools = 0,

        [Description("UPS (WorldShip)")]
        [ShipmentTypeIcon("ups")]
        UpsWorldShip = 1,

        [Description("USPS (Endicia)")]
        [ShipmentTypeIcon("endicia")]
        Endicia = 2,

        [Description("USPS (w/o Postage)")]
        [ShipmentTypeIcon("usps")]
        PostalWebTools = 4,

        [CallbackDescription("ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Express1EndiciaShipmentType", "ShipmentTypeName")]
        [ShipmentTypeIcon("express1")]
        Express1Endicia = 9,

        [CallbackDescription("ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Express1UspsShipmentType", "ShipmentTypeName")]
        [ShipmentTypeIcon("express1")]
        Express1Usps = 13,

        [Description("FedEx")]
        [ShipmentTypeIcon("fedex")]
        FedEx = 6,

        [Description("OnTrac")]
        [ShipmentTypeIcon("ontrac")]
        OnTrac = 11,

        [Description("i-parcel")]
        [ShipmentTypeIcon("iparcel")]
        iParcel = 12,

        [Description("Other")]
        [ShipmentTypeIcon("other")]
        Other = 5,

        [Description("Best Rate")]
        [ShipmentTypeIcon("bestrate")]
        BestRate = 14,

        [Description("USPS")]
        [ShipmentTypeIcon("usps")]
        Usps = 15,

        [Description("Amazon")]
        [ShipmentTypeIcon("amazon")]
        Amazon = 16,

        [Description("DHL Express")]
        [ShipmentTypeIcon("dhl")]
        DhlExpress = 16,

        [Description("None")]
        None = 99
    }
}
