using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Handles uploading data to Infopia
    /// </summary>
    public interface IInfopiaOnlineUpdater
    {
        /// <summary>
        /// Posts the tracking number for the identified shipment to the Infopia store
        /// </summary>
        Task UploadShipmentDetails(IInfopiaStoreEntity store, long shipmentID);

        /// <summary>
        /// Posts the tracking number for the identified shipment to the infopia store
        /// </summary>
        Task UploadShipmentDetails(IInfopiaStoreEntity store, ShipmentEntity shipment);

        /// <summary>
        /// Changes the status of an Infopia order to that specified.
        /// </summary>
        Task UpdateOrderStatus(IInfopiaStoreEntity store, long orderID, string status);

        /// <summary>
        /// Changes the status of an Infopia order to that specified.
        /// </summary>
        Task UpdateOrderStatus(IInfopiaStoreEntity store, long orderID, string status, UnitOfWork2 unitOfWork);
    }
}