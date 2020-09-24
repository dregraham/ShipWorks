using System.Reflection;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    /// <summary>
    /// DTO for importing ups account data from the hub
    /// </summary>
    [Obfuscation]
    public class UpsAccountConfiguration
    {
        /// <summary>
        /// The UPS access token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The UPS account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The UPS rate type
        /// </summary>
        public UpsRateType RateType { get; set; }
    }
}