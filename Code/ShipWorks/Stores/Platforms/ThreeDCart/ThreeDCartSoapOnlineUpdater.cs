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

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Updates ThreeDCart order status/shipments
    /// </summary>
    public class ThreeDCartSoapOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartSoapOnlineUpdater));
        readonly ThreeDCartStoreEntity threeDCartStore;

        // status code provider
        ThreeDCartStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartSoapOnlineUpdater(ThreeDCartStoreEntity store)
        {
            threeDCartStore = store;
        }

        /// <summary>
        /// Gets the status code provider
        /// </summary>
        protected ThreeDCartStatusCodeProvider StatusCodeProvider
        {
            get
            {
                if (statusCodeProvider == null)
                {
                    statusCodeProvider = new ThreeDCartStatusCodeProvider(threeDCartStore);
                }

                return statusCodeProvider;
            }
        }

        /// <summary>
        /// Changes the status of an ThreeDCart order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusCode)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UpdateOrderStatus(orderID, statusCode, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an ThreeDCart order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (!order.IsManual)
                {
                    ThreeDCartWebClient client = new ThreeDCartWebClient(threeDCartStore, null);
                    client.UpdateOrderStatus(order.OrderNumber, order.OrderNumberComplete, statusCode);

                    // Update the local database with the new status
                    OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
                    basePrototype.OnlineStatusCode = statusCode;
                    basePrototype.OnlineStatus = StatusCodeProvider.GetCodeName(statusCode);

                    unitOfWork.AddForSave(basePrototype);
                }
                else
                {
                    log.InfoFormat("Not uploading order status since order {0} is manual.", order.OrderID);
                }
            }
            else
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
            }
        }

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        public void UpdateShipmentDetails(OrderEntity order)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment == null)
            {
                // log that there was no shipment, and return
                log.DebugFormat("There was no shipment found for order Id: {0}", order.OrderID);
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.WarnFormat("Not updating status of shipment {0} as it has gone away.", shipmentID);
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        private void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            OrderEntity order = shipment.Order;
            if (order.IsManual)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", shipment.Order.OrderNumberComplete);
                return;
            }

            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            ThreeDCartWebClient webClient = new ThreeDCartWebClient(threeDCartStore, null);
            webClient.UploadOrderShipmentDetails(shipment);
        }
    }
}
