using System;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.SparkPay.DTO;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// The SparkPay web client
    /// </summary>
    public interface ISparkPayWebClient
    {
        /// <summary>
        /// Adds the given shipment to the order
        /// </summary>
        Task AddShipment(ISparkPayStoreEntity store, ShipmentEntity shipmentEntity, long orderNumber);

        /// <summary>
        /// Gets the address of the given Id
        /// </summary>
        AddressesResponse GetAddress(ISparkPayStoreEntity store, int addressId, IProgressReporter progressReporter);

        /// <summary>
        /// Gets orders that have an updated_at that is greater than the given start
        /// </summary>
        OrdersResponse GetOrders(ISparkPayStoreEntity store, DateTime start, IProgressReporter progressReporter);

        /// <summary>
        /// Gets a list of order statuses for the given store
        /// </summary>
        OrderStatusResponse GetStatuses(ISparkPayStoreEntity store);

        /// <summary>
        /// Gets the stores from the given URL
        /// </summary>
        StoresResponse GetStores(string token, string url);

        /// <summary>
        /// Updates the orders status
        /// </summary>
        Order UpdateOrderStatus(ISparkPayStoreEntity store, long orderNumber, int statusId);
    }
}