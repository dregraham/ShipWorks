using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    /// <summary>
    /// DTO for importing usps account data from the hub
    /// </summary>
    [Obfuscation]
    public class UspsConfigurationAccount
    {
        /// <summary>
        /// The account username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The account password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The account email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The account id
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// The account contract type
        /// </summary>
        public int ContractType { get; set; }

        /// <summary>
        /// The accounts global post availability
        /// </summary>
        public int GlobalPost { get; set; }

        /// <summary>
        /// The accounts ShipEngineId
        /// </summary>
        public string ShipEngineId { get; set; }
    }
}
