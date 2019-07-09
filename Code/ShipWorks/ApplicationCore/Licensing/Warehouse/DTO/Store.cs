using System.Reflection;
using Newtonsoft.Json;

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
    }
}
