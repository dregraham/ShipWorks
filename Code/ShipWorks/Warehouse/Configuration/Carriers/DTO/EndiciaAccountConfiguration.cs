using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.Carriers.DTO
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
        public string AccountNumber { get; set; }

        /// <summary>
        /// The Endicia passphrase
        /// </summary>
        public string Passphrase { get; set; }

        /// <summary>
        /// The Endicia Signature
        /// </summary>
        public string Signature { get; set; }
    }
}