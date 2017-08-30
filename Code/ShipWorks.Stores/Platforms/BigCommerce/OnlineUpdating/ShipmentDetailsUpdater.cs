using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Updates BigCommerce order status/shipments
    /// </summary>
    [Component]
    public class ShipmentDetailsUpdater : IShipmentDetailsUpdater
    {
        private readonly ILog log;
        private readonly IDataAccess dataAccess;
        private readonly IItemLoader productLoader;
        private readonly IOrderStatusUpdater orderUpdater;
        private readonly IShipmentDetailsUpdaterClient updateClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDetailsUpdater(
            IDataAccess dataAccess,
            IShipmentDetailsUpdaterClient updateClient,
            IItemLoader productLoader,
            IOrderStatusUpdater orderUpdater,
            Func<Type, ILog> createLogger)
        {
            this.updateClient = updateClient;
            this.orderUpdater = orderUpdater;
            this.productLoader = productLoader;
            this.dataAccess = dataAccess;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        public async Task UpdateShipmentDetailsForOrder(BigCommerceStoreEntity store, long orderID)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = await dataAccess.GetLatestActiveShipmentAsync(orderID).ConfigureAwait(false);
            if (shipment == null)
            {
                // log that there was no shipment, and return
                log.DebugFormat("There was no shipment found for order Id: {0}", orderID);
                return;
            }

            var orderDetails = await dataAccess.GetOrderDetailsAsync(orderID).ConfigureAwait(false);
            await UpdateShipmentDetails(store, shipment, orderDetails).ConfigureAwait(false);
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public async Task UpdateShipmentDetails(BigCommerceStoreEntity store, long shipmentID)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            ShipmentEntity shipment = await dataAccess.GetShipmentAsync(shipmentID).ConfigureAwait(false);

            if (shipment == null)
            {
                log.WarnFormat("Not updating status of shipment {0} as it has gone away.", shipmentID);
                return;
            }

            var orderDetails = await dataAccess.GetOrderDetailsAsync(shipment.OrderID).ConfigureAwait(false);

            if (orderDetails == null)
            {
                log.WarnFormat("Not updating order {0} since it has gone away.", shipment.OrderID);
                return;
            }

            await UpdateShipmentDetails(store, shipment, orderDetails).ConfigureAwait(false);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        private async Task UpdateShipmentDetails(BigCommerceStoreEntity store, ShipmentEntity shipment, OnlineOrder orderDetails)
        {
            if (orderDetails.AreAllManual)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", orderDetails.OrderNumberComplete);
                return;
            }

            IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>> allItems = await dataAccess.GetOrderItemsAsync(orderDetails.OrderID).ConfigureAwait(false);

            // BigCommerce requires order items to create a shipment, so make sure we have some
            if (allItems?.SelectMany(x => x.Value).Any() != true)
            {
                throw new BigCommerceException(string.Format("Unable to upload shipment details because no order items were found for order number {0}", orderDetails.OrderNumberComplete));
            }

            // If the order is only digital, update the order status to completed, then return
            // We don't want to actually "ship" the order because there is no order address id, and BigCommerce
            // will return an error.
            if (updateClient.IsOrderAllDigital(allItems.SelectMany(x => x.Value)))
            {
                await orderUpdater.UpdateOrderStatus(store, orderDetails, BigCommerceConstants.OrderStatusCompleted).ConfigureAwait(false);
                return;
            }

            var allResults = new List<IResult>();
            foreach (var orderDetail in orderDetails.OrdersToUpload)
            {
                var result = await Result.Handle<BigCommerceException>()
                    .ExecuteAsync(() => updateClient.UpdateOnline(store, orderDetail, orderDetails.OrderNumberComplete, shipment, allItems))
                    .ConfigureAwait(false);

                allResults.Add(result);
            }

            allResults.ThrowIfNotEmpty((msg, ex) => new BigCommerceException(msg, ex));
        }
    }
}
