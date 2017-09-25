using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Updates BigCommerce order status/shipments
    /// </summary>
    public interface IBigCommerceShipmentDetailsUpdater
    {
        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        Task UpdateShipmentDetailsForOrder(BigCommerceStoreEntity store, long orderID);

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        Task UpdateShipmentDetails(BigCommerceStoreEntity store, long shipmentID);
    }
}