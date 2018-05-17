using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Overstock.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading orders to Overstock
    /// </summary>
    public class OverstockDataAccess : IOverstockDataAccess
    {
        /// <summary>
        /// Get details of orders for the given shipments
        /// </summary>
        public Task<IEnumerable<OverstockSupplierShipment>> GetOrderDetails(IShipmentEntity shipmentEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}