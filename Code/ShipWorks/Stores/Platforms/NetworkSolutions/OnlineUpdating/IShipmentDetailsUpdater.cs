using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// Update NetworkSolutions shipment details online
    /// </summary>
    public interface IShipmentDetailsUpdater
    {
        /// <summary>
        /// Uploads shipment details for the given shipment Id
        /// </summary>
        Task UploadShipmentDetails(INetworkSolutionsStoreEntity store, long shipmentID);

        /// <summary>
        /// Uploads shipment details for the given order Id
        /// </summary>
        Task UploadShipmentDetailsForOrder(INetworkSolutionsStoreEntity store, long orderID);
    }
}