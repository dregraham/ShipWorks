using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
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
        public long AccountNumber { get; set; }
    }
}