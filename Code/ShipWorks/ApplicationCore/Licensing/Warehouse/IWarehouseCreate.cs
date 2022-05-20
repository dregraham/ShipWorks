using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Create warehouse
    /// </summary>
    public interface IWarehouseCreate
    {
        /// <summary>
        /// Creates a warehouse on the hub and returns the id of the new warehouse
        /// </summary>
        Task<GenericResult<string>> Create(Details warehouse);
    }
}
