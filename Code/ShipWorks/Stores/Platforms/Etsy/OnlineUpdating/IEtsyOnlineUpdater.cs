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
        /// Update the online status of the given order
        /// </summary>
        Task UpdateOnlineStatus(EtsyOrderEntity order, bool? markAsPaid, bool? markAsShipped);

        /// <summary>
        /// Given shipmentID, send comment, markAsPad, and markAsShipped as applicable
        /// </summary>
        Task UpdateOnlineStatus(long shipmentID, bool? markAsPaid, bool? markAsShipped, string comment, UnitOfWork2 unitOfWork);

        /// <summary>
        /// Update the online status of the given shipment
        /// </summary>
        Task UpdateOnlineStatus(ShipmentEntity shipment, bool? markAsPaid, bool? markAsShipped, string comment);

        /// <summary>
        /// Upload the online status of the shipment
        /// </summary>
        Task UpdateOnlineStatus(ShipmentEntity shipment, bool? markAsPaid, bool? markAsShipped, string comment, UnitOfWork2 unitOfWork);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        Task UploadShipmentDetails(long orderID);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        Task UploadShipmentDetails(long shipmentID, UnitOfWork2 unitOfWork);
    }
}