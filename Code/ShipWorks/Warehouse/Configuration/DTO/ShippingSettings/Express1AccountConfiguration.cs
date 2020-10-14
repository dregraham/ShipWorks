using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    /// <summary>
    /// DTO for importing Express1 USPS account data from the hub
    /// </summary>
    [Obfuscation]
    public class Express1AccountConfiguration
    {
        /// <summary>
        /// The Express1 USPS username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The Express1 USPS password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The Customs Signature
        /// </summary>
        public string Signature { get; set; }
    }
}