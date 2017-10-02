using System.Collections.Generic;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Interface for AmeriCommerce online updater
    /// </summary>
    public interface IAmeriCommerceOnlineUpdater
    {
        /// <summary>
        /// Changes the status of an AmeriCommerce order to that specified
        /// </summary>
        Task UpdateOrderStatus(IAmeriCommerceStoreEntity store, long orderID, int statusCode);

        /// <summary>
        /// Changes the status of an AmeriCommerce order to that specified
        /// </summary>
        Task UpdateOrderStatus(IAmeriCommerceStoreEntity store, long orderID, int statusCode, UnitOfWork2 unitOfWork);

        // Upload shipment details for a list of orderIDs
        Task UploadOrderShipmentDetails(IAmeriCommerceStoreEntity store, IEnumerable<long> orderIDs);

        // Upload shipment details for a shipment
        Task UploadOrderShipmentDetails(IAmeriCommerceStoreEntity store, long shipmentID);
    }
}