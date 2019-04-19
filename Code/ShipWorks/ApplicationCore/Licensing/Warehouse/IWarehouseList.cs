using System;
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
        GenericResult<WarehouseListDto> GetList(TokenResponse tokenResponse);
    }
}
