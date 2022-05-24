using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce packaging types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DhlEcommercePackagingType
    {
        [Description("Irregular Parcel")]
        [ApiValue("dhl_ecommerce_irregular_parcel")]

        IrregularParcel = 0,

        [Description("Machinable Parcel")]
        [ApiValue("dhl_ecommerce_machinable_parcel")]

        MachinableParcel = 1,

        [Description("BPM Parcel")]
        [ApiValue("dhl_ecommerce_bpm_parcel")]

        BpmParcel = 2,

        [Description("Parcel Select Machinable")]
        [ApiValue("dhl_ecommerce_parcel_select_machinable")]

        ParcelSelectMachinable = 3,

        [Description("Parcel Select Non-Machinable")]
        [ApiValue("dhl_ecommerce_parcel_select_non_machinable")]

        ParcelSelectNonMachinable = 4,

        [Description("Media Mail")]
        [ApiValue("dhl_ecommerce_media_mail")]

        MediaMail = 5,

        [Description("Marketing Parcel < 6oz")]
        [ApiValue("dhl_ecommerce_marketing_parcel_lt_6oz")]

        MarketingParcelLessThanSixOunces = 6,

        [Description("Marketing Parcel >= 6oz")]
        [ApiValue("dhl_ecommerce_marketing_parcel_gte_6oz")]

        MarketingParcelGreaterThanOrEqualToSixOunces = 7,
    }
}
