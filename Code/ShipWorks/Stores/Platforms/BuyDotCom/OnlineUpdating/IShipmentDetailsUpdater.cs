using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Uploads tracking information on Buy.com
    /// </summary>
    public interface IShipmentDetailsUpdater
    {
        /// <summary>
        /// Upload ship confirmation to buy.com for shipment IDs
        /// </summary>
        Task UploadShipmentDetails(IBuyDotComStoreEntity store, IEnumerable<long> shipmentKeys);
    }
}
