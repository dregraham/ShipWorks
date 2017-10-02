using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Uploads shipment details and order status to Yahoo
    /// </summary>
    public interface IYahooApiOnlineUpdater
    {
        /// <summary>
        /// Changes the status of a Yahoo order to that specified
        /// </summary>
        Task UpdateOrderStatus(IYahooStoreEntity store, long orderID, string status);

        /// <summary>
        /// Updates the shipment details.
        /// </summary>
        Task UpdateShipmentDetails(IYahooStoreEntity store, long orderKey);

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        Task UpdateShipmentDetails(IYahooStoreEntity store, ShipmentEntity shipment);
    }
}