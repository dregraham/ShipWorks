using System.ComponentModel;
using System.Reflection;
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
        [WpfImageSource("/ShipWorks.UI;component/Resources/ups.png")]
        UpsOnLineTools = 0,

        [Description("UPS (WorldShip)")]
        [ShipmentTypeIcon("ups")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/ups.png")]
        UpsWorldShip = 1,

        [Description("USPS (Endicia)")]
        [ShipmentTypeIcon("endicia")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/endicia.png")]
        Endicia = 2,

        [Description("USPS (w/o Postage)")]
        [ShipmentTypeIcon("usps")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/usps.png")]
        PostalWebTools = 4,

        [CallbackDescription("ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Express1EndiciaShipmentType", "ShipmentTypeName")]
        [ShipmentTypeIcon("express1")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/express1.png")]
        Express1Endicia = 9,

        [CallbackDescription("ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Express1UspsShipmentType", "ShipmentTypeName")]
        [ShipmentTypeIcon("express1")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/express1.png")]
        Express1Usps = 13,

        [Description("FedEx")]
        [ShipmentTypeIcon("fedex")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/fedex.png")]
        FedEx = 6,

        [Description("OnTrac")]
        [ShipmentTypeIcon("ontrac")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/ontrac.png")]
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
        [WpfImageSource("/ShipWorks.UI;component/Resources/usps.png")]
        Usps = 15,

        [Description("Amazon Buy Shipping API")]
        [ShipmentTypeIcon("amazon")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/amazon.png")]
        AmazonSFP = 16,

        [Description("DHL Express")]
        [ShipmentTypeIcon("dhl")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/dhl.png")]
        DhlExpress = 17,

        [Description("Asendia")]
        [ShipmentTypeIcon("asendia")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/asendia.png")]
        Asendia = 18,

        [Description("Amazon Shipping")]
        [ShipmentTypeIcon("amazonswa")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/amazon.png")]
        AmazonSWA = 19,

        [Description("DHL eCommerce")]
        [ShipmentTypeIcon("dhl")]
        [WpfImageSource("/ShipWorks.UI;component/Resources/dhl.png")]
        DhlEcommerce = 20,

        [Description("None")]
        None = 99
    }
}
