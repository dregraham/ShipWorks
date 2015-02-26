using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Editions
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipmentTypeRestrictionType
    {
        /// <summary>
        /// The entire shipment type is restricted/disabled from being used in ShipWorks.
        /// </summary>
        [Description("Disabled")]
        [ApiValue("Disabled")]
        Disabled = 0,

        /// <summary>
        /// New accounts cannot be registered/created in ShipWorks.
        /// </summary>
        [Description("Account registration")]
        [ApiValue("AccountRegistration")]
        AccountRegistration = 1,

        /// <summary>
        /// A shipment cannot be processed.
        /// </summary>
        [Description("Processing")]
        [ApiValue("Processing")]
        Processing = 2,

        /// <summary>
        /// Purchasing postage is not allowed.
        /// </summary>
        [Description("Purchasing")]
        [ApiValue("Purchasing")]
        Purchasing = 3,

        /// <summary>
        /// Footers for discount messaging in the rate control are not allowed.
        /// </summary>
        [Description("Rate discount messaging")]
        [ApiValue("RateDiscountMessaging")]
        RateDiscountMessaging = 4,

        /// <summary>
        /// Dont' show messaging to convert/upgrade an existing Stamp.com account. This is sort of out of place 
        /// and pertains only to USPS. This is a result of a problem on the Stamps.com side when 
        /// USPS customers have multi-user accounts where they don't want to allow these customers
        /// to convert through ShipWorks. After USPS has reached out to these customers and converted
        /// their accounts, this can be removed.
        /// </summary>
        [Description("Shipping account conversion")]
        [ApiValue("ShippingAccountConversion")]
        ShippingAccountConversion = 5
    }
}
