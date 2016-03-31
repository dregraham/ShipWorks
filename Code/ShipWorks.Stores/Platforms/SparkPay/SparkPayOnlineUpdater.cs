using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using ShipWorks.Stores.Platforms.SparkPay.Factories;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    ///     Uploads shipment information to SparkPay
    /// </summary>
    public class SparkPayOnlineUpdater
    {
        // the store this instance is for
        private readonly SparkPayStoreEntity store;
        private readonly SparkPayWebClient webClient;
        private readonly StatusCodeProvider<int> statusCodeProvider;
        private readonly IOrderManager orderManager;
        private readonly SparkPayShipmentFactory shipmentFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayOnlineUpdater(
            SparkPayStoreEntity store,
            SparkPayWebClient webClient, 
            Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory, 
            IOrderManager orderManager, 
            SparkPayShipmentFactory shipmentFactory)
        {
            this.store = store;
            this.webClient = webClient;
            statusCodeProvider = statusCodeProviderFactory(store);
            this.orderManager = orderManager;
            this.shipmentFactory = shipmentFactory;
        }

        /// <summary>
        /// Changes the status of an SparkPay order to that specified
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
        /// Changes the status of an SparkPay order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = orderManager.FetchOrder(orderID);

            if (order != null && !order.IsManual)
            {
                Order orderResponse = webClient.UpdateOrderStatus(store, order.OrderNumber, statusCode);

                order.OnlineStatusCode = orderResponse.OrderStatusId;
                order.OnlineStatus = statusCodeProvider.GetCodeName((int)orderResponse.OrderStatusId);
                
                // Update the local database with the new status
                OrderEntity basePrototype = new OrderEntity(orderID)
                {
                    IsNew = false,
                    OnlineStatusCode = statusCode,
                    OnlineStatus = statusCodeProvider.GetCodeName(statusCode)
                };

                unitOfWork.AddForSave(basePrototype);
            }
        }

        /// <summary>
        ///     Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);

                // Check to see if shipment exists
                if (shipment == null)
                {
                    continue;
                }

                if (!shipment.Order.IsManual)
                {
                    webClient.AddShipment(store, shipmentFactory.Create(shipment));
                }
            }
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException(nameof(shipment));
            }

            if (!shipment.Order.IsManual)
            {
                webClient.AddShipment(store, shipmentFactory.Create(shipment));
            }
        }
    }
}