using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Rakuten.OnlineUpdating
{
    /// <summary>
    /// Posts shipping information
    /// </summary>
    public interface IRakutenOnlineUpdater
    {
        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        Task UploadTrackingNumber(IRakutenStoreEntity store, long shipmentID);

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        Task UploadTrackingNumber(IRakutenStoreEntity store, ShipmentEntity shipment);
    }
}