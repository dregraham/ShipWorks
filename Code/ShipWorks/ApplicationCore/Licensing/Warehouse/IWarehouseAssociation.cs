using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Associate this instance of ShipWorks with a warehouse
    /// </summary>
    public interface IWarehouseAssociation
    {
        /// <summary>
        /// Get list of warehouses
        /// </summary>
        Task<Result> Associate(string warehouseId);
    }
}