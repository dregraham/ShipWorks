using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    /// <summary>
    /// DTO for importing Endicia account data from the hub
    /// </summary>
    [Obfuscation]
    public class EndiciaAccountConfiguration
    {
        /// <summary>
        /// The Endicia account number
        /// </summary>
        public string accountNumber { get; set; }

        /// <summary>
        /// The Endicia passphrase
        /// </summary>
        public string password { get; set; }
    }
}