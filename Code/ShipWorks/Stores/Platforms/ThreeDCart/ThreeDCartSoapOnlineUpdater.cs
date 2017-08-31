using System;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using log4net;
using Interapptive.Shared.Enums;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating;
using System.Collections.Generic;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Updates ThreeDCart order status/shipments
    /// </summary>
    [Component(RegisterAs = RegistrationType.Self)]
    public class ThreeDCartSoapOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartSoapOnlineUpdater));
        private readonly ThreeDCartStoreEntity threeDCartStore;
        private readonly ThreeDCartWebClient webClient;

        // status code provider
        private ThreeDCartStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartSoapOnlineUpdater(ThreeDCartStoreEntity store, Func<ThreeDCartStoreEntity, IProgressReporter, ThreeDCartWebClient> webClientFactory)
        {
            threeDCartStore = store;
            webClient = webClientFactory(threeDCartStore, null);
        }

        /// <summary>
        /// Gets the status code provider
        /// </summary>
        protected ThreeDCartStatusCodeProvider StatusCodeProvider =>
            statusCodeProvider ?? (statusCodeProvider = new ThreeDCartStatusCodeProvider(threeDCartStore));

        /// <summary>
        /// Changes the status of an ThreeDCart order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(long orderID, int statusCode)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOrderStatus(orderID, statusCode, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an ThreeDCart order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);

            if (order == null)
            {
                log.WarnFormat($"Unable to update online status for order {orderID}: Unable to find order");
                return;
            }

            if (order.IsManual && order.CombineSplitStatus == CombineSplitStatusType.None)
            {
                log.InfoFormat($"Not uploading order status since order {order.OrderNumberComplete} is manual.");
                return;
            }

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IThreeDCartOnlineUpdatingDataAccess dataAccess = scope.Resolve<IThreeDCartOnlineUpdatingDataAccess>();
                IEnumerable<ThreeDCartOnlineUpdatingOrderDetail> orderDetails = await dataAccess.GetOrderDetails(orderID).ConfigureAwait(false);

                foreach (ThreeDCartOnlineUpdatingOrderDetail orderDetail in orderDetails)
                {
                    webClient.UpdateOrderStatus(orderDetail.OrderNumber, orderDetail.OrderNumberComplete, statusCode);
                }

                // Update the local database with the new status
                OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
                basePrototype.OnlineStatusCode = statusCode;
                basePrototype.OnlineStatus = StatusCodeProvider.GetCodeName(statusCode);

                unitOfWork.AddForSave(basePrototype);
            }
        }
        
        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public async Task UpdateShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.WarnFormat("Not updating status of shipment {0} as it has gone away.", shipmentID);
                return;
            }

            await UpdateShipmentDetails(shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        private async Task UpdateShipmentDetails(ShipmentEntity shipment)
        {
            OrderEntity order = shipment.Order;
            if (order.IsManual && order.CombineSplitStatus == CombineSplitStatusType.None)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", shipment.Order.OrderNumberComplete);
                return;
            }

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IThreeDCartOnlineUpdatingDataAccess dataAccess = scope.Resolve<IThreeDCartOnlineUpdatingDataAccess>();
                IEnumerable<ThreeDCartOnlineUpdatingOrderDetail> orderDetails = await dataAccess.GetOrderDetails(order.OrderID).ConfigureAwait(false);

                foreach (ThreeDCartOnlineUpdatingOrderDetail orderDetail in orderDetails)
                {
                    long shipmentID = await dataAccess.GetFirstItemShipmentIDByOriginalOrderID(orderDetail.OriginalOrderID).ConfigureAwait(false);

                    webClient.UploadOrderShipmentDetails(orderDetail, shipmentID, shipment.TrackingNumber);
                }
            }
        }
    }
}
