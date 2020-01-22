using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// A ShipEngine Store
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class ShipEngineStore : Store
    {
        /// <summary>
        /// The Order Source Id of the ShipEngine Store
        /// </summary>
        public string OrderSourceId { get; set; }

        /// <summary>
        /// The Account Id of the ShipEngine Store
        /// </summary>
        public string AccountId { get; set; }
    }
}
