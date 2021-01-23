using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.Carriers.DTO
{
    /// <summary>
    /// DTO for importing FedEx account data from the hub
    /// </summary>
    [Obfuscation]
    public class FedExAccountConfiguration
    {
        /// <summary>
        /// The FedEx account number
        /// </summary>
        public string AccountNumber { get; set; }
    }
}