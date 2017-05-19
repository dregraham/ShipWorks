using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Describes how to get a UPS rate
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsRatingMethod
    {
        /// <summary>
        /// This describes the method of getting rates via the UPS API
        /// </summary>
        ApiOnly,

        /// <summary>
        /// This describes the method of calculating rates in Shipworks using the rating tables, 
        /// failing over to the API if local rating returns no rates
        /// </summary>
        LocalWithApiFailover,

        /// <summary>
        /// This describes the method of calculating rates in Shipworks using the rating tables
        /// </summary>
        LocalOnly
    }
}
