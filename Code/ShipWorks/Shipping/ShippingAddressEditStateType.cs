using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Enum describing state of editability of the shipping address
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShippingAddressEditStateType
    {
        /// <summary>
        /// The shipping address is editable
        /// </summary>
        [Description("Editable")]
        Editable = 0,

        /// <summary>
        /// The user does not have permission to edit this shipment
        /// </summary>
        [Description("Permission denied")]
        PermissionDenied = 1,

        /// <summary>
        /// The shipment is processed
        /// </summary>
        [Description("Processed")]
        Processed = 2,

        /// <summary>
        /// The shipment is to be fulfilled by eBay GSP
        /// </summary>
        [Description("eBay GSP")]
        GspFulfilled = 3,

        /// <summary>
        /// The shipment is to be printed from Amazons carriers
        /// </summary>
        [Description("Amazon Buy Shipping API")]
        AmazonSfp = 4,
    }
}
