using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Overstock.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading orders to Overstock
    /// </summary>
    public interface IOverstockDataAccess
    {
        /// <summary>
        /// Get details of orders for the given shipments
        /// </summary>
        Task<IEnumerable<OverstockSupplierShipment>> GetOrderDetails(IEnumerable<IShipmentEntity> shipmentEntity);
    }
}