using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Link this instance of ShipWorks with a warehouse
    /// </summary>
    public interface IWarehouseLink
    {
        /// <summary>
        /// Get list of warehouses
        /// </summary>
        Task<Result> Link(string warehouseId);
    }
}