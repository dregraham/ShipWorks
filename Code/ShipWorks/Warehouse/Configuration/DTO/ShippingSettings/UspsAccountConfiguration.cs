using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    /// <summary>
    /// DTO for importing usps account data from the hub
    /// </summary>
    [Obfuscation]
    public class UspsAccountConfiguration
    {
        /// <summary>
        /// The account username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The account password
        /// </summary>
        public string Password { get; set; }
    }
}
