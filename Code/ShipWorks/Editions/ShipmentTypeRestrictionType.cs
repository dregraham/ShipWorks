using System.Reflection;

namespace ShipWorks.Editions
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShipmentTypeRestrictionType
    {
        /// <summary>
        /// The entire shipment type is restricted/disabled from being used in ShipWorks.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// New accounts cannot be registered/created in ShipWorks.
        /// </summary>
        AccountRegistration = 1,

        /// <summary>
        /// A shipment cannot be processed.
        /// </summary>
        Processing = 2,

        /// <summary>
        /// Purchasing postage is not allowed.
        /// </summary>
        Purchasing = 3,

        /// <summary>
        /// Footers for discount messaging in the rate control are not allowed.
        /// </summary>
        RateDiscountMessaging = 4,

        /// <summary>
        /// Dont' show messaging to convert/upgrade an existing Stamp.com account. This is sort of out of place 
        /// and pertains only to Stamps.com. This is a result of a problem on the Stamps.com side when 
        /// Stamps.com customers have multi-user accounts where they don't want to allow these customers
        /// to convert through ShipWorks. After Stamps.com has reached out to these customers and converted
        /// their accounts, this can be removed.
        /// </summary>
        ShippingAccountConversion = 5
    }
}
