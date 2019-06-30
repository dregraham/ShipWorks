using System.Reflection;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// View model that represents a warehouse
    /// </summary>
    public interface IWarehouseViewModel
    {
        /// <summary>
        /// Name of the warehouse
        /// </summary>
        [Obfuscation]
        string Name { get; }
    }
}