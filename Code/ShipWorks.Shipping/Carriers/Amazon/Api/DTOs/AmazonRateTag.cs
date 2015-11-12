using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    /// <summary>
    /// Amazon rate info
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class AmazonRateTag
    {
        /// <summary>
        /// The shipping service id of the rate
        /// </summary>
        public string ShippingServiceId { get; set; }

        /// <summary>
        /// The shipping service offer id of the rate
        /// </summary>
        public string ShippingServiceOfferId { get; set; }

        /// <summary>
        /// The description of the rate
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The carrier name of the rate
        /// </summary>
        public string CarrierName { get; set; }
    }
}
