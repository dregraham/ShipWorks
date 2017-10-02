using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Web client for interfacing with the NetworkSolutions SOAP api
    /// </summary>
    public interface INetworkSolutionsWebClient
    {
        /// <summary>
        /// Update the online status of an order
        /// </summary>
        void UpdateOrderStatus(NetworkSolutionsStoreEntity store, long networkSolutionsOrderId, long currentStatus, long targetStatus, string comments);

        /// <summary>
        /// Uploads the tracking number for a shipment
        /// </summary>
        void UploadShipmentDetails(INetworkSolutionsStoreEntity store, long networkSolutionsOrderID, ShipmentEntity shipment);
    }
}