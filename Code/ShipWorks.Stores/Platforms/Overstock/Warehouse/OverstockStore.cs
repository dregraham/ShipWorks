using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Platforms.Overstock.Warehouse
{
    /// <summary>
    /// Overstock Store DTO
    /// </summary>
    [Obfuscation]
    public class OverstockStore : Store
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockStore()
        {
        }

        /// <summary>
        /// Username for the store
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password for the store
        /// </summary>
        public string Password { get; set; }
    }
}