using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Sears.OnlineUpdating
{
    /// <summary>
    /// Online updater
    /// </summary>
    public interface ISearsOnlineUpdater
    {
        /// <summary>
        /// Upload the shipment details for the given shipment ID
        /// </summary>
        Task UploadShipmentDetails(ISearsStoreEntity store, long shipmentID);

        /// <summary>
        /// Upload the shipment details for the given shipment
        /// </summary>
        Task UploadShipmentDetails(ISearsStoreEntity store, ShipmentEntity shipment);
    }
}