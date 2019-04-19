using System.Collections.Generic;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// View model for the warehouse list dialog
    /// </summary>
    public interface IWarehouseListViewModel
    {
        /// <summary>
        /// Choose a warehouse to associate ShipWorks with
        /// </summary>
        WarehouseViewModel ChooseWarehouse(IEnumerable<WarehouseViewModel> warehouses);
    }
}