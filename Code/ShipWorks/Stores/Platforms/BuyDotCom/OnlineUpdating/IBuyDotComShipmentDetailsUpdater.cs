using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Uploads tracking information on Buy.com
    /// </summary>
    public interface IBuyDotComShipmentDetailsUpdater
    {
        /// <summary>
        /// Upload ship confirmation to buy.com for order IDs
        /// </summary>
        Task UploadShipmentDetailsForOrders(IBuyDotComStoreEntity store, IEnumerable<long> orderKeys);

        /// <summary>
        /// Upload ship confirmation to buy.com for shipment IDs
        /// </summary>
        Task UploadShipmentDetailsForShipments(IBuyDotComStoreEntity store, IEnumerable<long> shipmentKeys);
    }
}
