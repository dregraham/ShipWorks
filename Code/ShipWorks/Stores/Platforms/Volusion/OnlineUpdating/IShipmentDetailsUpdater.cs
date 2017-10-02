using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Volusion.OnlineUpdating
{
    /// <summary>
    /// Handles uploading shipment details to Volusion
    /// </summary>
    public interface IShipmentDetailsUpdater
    {
        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        Task UploadShipmentDetails(IVolusionStoreEntity store, ShipmentEntity shipment, bool sendEmail);

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        Task UploadShipmentDetails(IVolusionStoreEntity store, ShipmentEntity shipment, bool sendEmail, UnitOfWork2 unitOfWork);
    }
}
