using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Etsy.OnlineUpdating
{
    /// <summary>
    /// Updates payment and tracking information to Etsy.
    /// </summary>
    public interface IEtsyOnlineUpdater
    {
        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        Task UploadShipmentDetails(EtsyStoreEntity store, long orderID);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        Task UploadShipmentDetails(EtsyStoreEntity store, long shipmentID, UnitOfWork2 unitOfWork);
    }
}