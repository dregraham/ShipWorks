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
        Result Associate(string warehouseId);
    }
}