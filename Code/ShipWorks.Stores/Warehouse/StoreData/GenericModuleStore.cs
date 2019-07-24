using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// GenericModule store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class GenericModuleStore : Store
    {
        /// <summary>
        /// The username to access the store
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password to access the store
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The URL of the store
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// How far back to start importing
        /// </summary>
        public ulong? ImportStartDetails { get; set; }

        /// <summary>
        /// Store code
        /// </summary>
        public string OnlineStoreCode { get; set; }
    }
}