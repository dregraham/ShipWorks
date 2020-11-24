using System.Reflection;

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
    }
}
