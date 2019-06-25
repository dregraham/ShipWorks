using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Get a list of warehouses
    /// </summary>
    public interface IWarehouseList
    {
        /// <summary>
        /// Get a list of warehouses
        /// </summary>
        Task<GenericResult<WarehouseListDto>> GetList();
    }
}
