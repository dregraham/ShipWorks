using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.Carriers.DTO
{
    /// <summary>
    /// DTO for importing DHL account data from the hub
    /// </summary>
    [Obfuscation]
    public class DhlAccountConfiguration
    {
        /// <summary>
        /// The DHL account number
        /// </summary>
        public string AccountNumber { get; set; }
    }
}
