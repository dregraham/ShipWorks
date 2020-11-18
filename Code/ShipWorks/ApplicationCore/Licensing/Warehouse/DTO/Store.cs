using System.Reflection;
using ShipWorks.Warehouse.Configuration.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class Store
    {
        /// <summary>
        /// The type of store this is
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of store this is
        /// </summary>
        public int StoreType { get; set; }

        /// <summary>
        /// The unique identifier for the store
        /// </summary>
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// The warehouse that migrated this store to the hub
        /// </summary>
        public string MigrationWarehouseId { get; set; }

        /// <summary>
        /// The store's address
        /// </summary>
        public ConfigurationAddress Address { get; set; }

        /// <summary>
        /// Whether or not this store should be synced with other instances of ShipWorks
        /// </summary>
        public bool ShipWorksSyncEnabled { get; set; }
    }
}
