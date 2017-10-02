using System.Collections.Generic;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Data access for the BigCommerce online updater
    /// </summary>
    public interface IBigCommerceDataAccess
    {
        /// <summary>
        /// Get a unit of work
        /// </summary>
        IUnitOfWorkCore GetUnitOfWork();

        /// <summary>
        /// Commit a unit of work
        /// </summary>
        Task Commit(IUnitOfWorkCore unitOfWork);

        /// <summary>
        /// Get order details for uploading
        /// </summary>
        Task<BigCommerceOnlineOrder> GetOrderDetailsAsync(long orderID);

        /// <summary>
        /// Get the latest active shipment
        /// </summary>
        Task<ShipmentEntity> GetLatestActiveShipmentAsync(long orderID);

        /// <summary>
        /// Get a specified shipment
        /// </summary>
        Task<ShipmentEntity> GetShipmentAsync(long shipmentID);

        /// <summary>
        /// Get order items
        /// </summary>
        Task<IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>>> GetOrderItemsAsync(long orderID);

        /// <summary>
        /// Get the overridden service used for a shipment
        /// </summary>
        string GetOverriddenServiceUsed(ShipmentEntity shipment);
    }
}