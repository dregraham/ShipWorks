using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// Volusion store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class VolusionStore : Store
    {
        /// <summary>
        /// The URL of the Volusion store
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// The username to login to the store
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The api password
        /// </summary>
        public string EncryptedPassword { get; set; }
}
