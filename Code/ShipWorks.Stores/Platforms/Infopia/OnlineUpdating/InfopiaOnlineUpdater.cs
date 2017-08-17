using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Infopia.OnlineUpdating
{
    /// <summary>
    /// Handles uploading data to Infopia
    /// </summary>
    [Component]
    public class InfopiaOnlineUpdater : IInfopiaOnlineUpdater
    {
        private readonly ILog log;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShippingManager shippingManager;
        private readonly IDataProvider dataProvider;
        private readonly ICombineOrderNumberSearchProvider orderNumberSearchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaOnlineUpdater(IShippingManager shippingManager,
            IDataProvider dataProvider,
            ISqlAdapterFactory sqlAdapterFactory,
            ICombineOrderNumberSearchProvider orderNumberSearchProvider,
            Func<Type, ILog> createLogger)
        {
            this.orderNumberSearchProvider = orderNumberSearchProvider;
            this.dataProvider = dataProvider;
            this.shippingManager = shippingManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Posts the tracking number for the identified shipment to the Infopia store
        /// </summary>
        public async Task UploadShipmentDetails(IInfopiaStoreEntity store, long shipmentID)
        {
            var adapter = await shippingManager.GetShipmentAsync(shipmentID).ConfigureAwait(false);
            if (adapter?.Shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            await UploadShipmentDetails(store, adapter.Shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Posts the tracking number for the identified shipment to the infopia store
        /// </summary>
        public async Task UploadShipmentDetails(IInfopiaStoreEntity store, ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;
            if (order.IsManual)
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
                return;
            }

            // Upload tracking number
            InfopiaWebClient client = new InfopiaWebClient(store);

            string trackingNumber = "";
            string shipper = "";
            decimal charge = shipment.ShipmentCost;

            var orderIdentifiers = await orderNumberSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            // This is wrapped in a task because it's too much effort (at this point) to convert the web client to be async.
            await Task.Run(() =>
            {
                InfopiaUtility.GetShipmentUploadValues(shipment, out shipper, out trackingNumber);

                foreach (long identifier in orderIdentifiers)
                {
                    client.UploadShipmentDetails(order.OrderNumber, shipper, trackingNumber, charge);
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Changes the status of an Infopia order to that specified.
        /// </summary>
        public async Task UpdateOrderStatus(IInfopiaStoreEntity store, long orderID, string status)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOrderStatus(store, orderID, status, unitOfWork).ConfigureAwait(false);

            using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
            {
                await unitOfWork.CommitAsync(adapter.AsDataAccessAdapter()).ConfigureAwait(false);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an Infopia order to that specified.
        /// </summary>
        public async Task UpdateOrderStatus(IInfopiaStoreEntity store, long orderID, string status, UnitOfWork2 unitOfWork)
        {
            IOrderEntity order = await dataProvider.GetEntityAsync<OrderEntity>(orderID).ConfigureAwait(false);
            if (order == null)
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
                return;
            }

            if (order.IsManual && order.CombineSplitStatus != CombineSplitStatusType.Combined)
            {
                log.WarnFormat("Not uploading shipment details since order {0} is manual.", order.OrderID);
                return;
            }


            InfopiaWebClient client = new InfopiaWebClient(store);
            var orderIdentifiers = await orderNumberSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            foreach (long identifier in orderIdentifiers)
            {
                // This is wrapped in a task because it's too much effort (at this point) to convert the web client to be async.
                await Task.Run(() => client.UpdateOrderStatus(identifier, status)).ConfigureAwait(false);
            }

            // Update the local database with the new status
            OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
            basePrototype.OnlineStatus = status;

            unitOfWork.AddForSave(basePrototype);
        }
    }
}
