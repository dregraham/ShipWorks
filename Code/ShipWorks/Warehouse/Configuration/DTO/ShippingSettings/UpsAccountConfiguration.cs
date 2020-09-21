using System.Reflection;

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
    }
}