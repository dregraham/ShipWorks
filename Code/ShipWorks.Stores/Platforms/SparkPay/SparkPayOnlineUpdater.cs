using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using ShipWorks.Stores.Platforms.SparkPay.Factories;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayOnlineUpdater(
            SparkPayStoreEntity store,
            SparkPayWebClient webClient,
            Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory,
            IOrderManager orderManager)
        {
            this.store = store;
            this.webClient = webClient;
            statusCodeProvider = statusCodeProviderFactory(store);
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Changes the status of an SparkPay order to that specified
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
        /// Changes the status of an SparkPay order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = orderManager.FetchOrder(orderID);

            if (order != null && (!order.IsManual || order.CombineSplitStatus == CombineSplitStatusType.Combined))
            {
                Order orderResponse = await webClient.UpdateOrderStatus(store, order, statusCode).ConfigureAwait(false);

                order.OnlineStatusCode = orderResponse.OrderStatusId;
                if (orderResponse.OrderStatusId != null)
                {
                    order.OnlineStatus = statusCodeProvider.GetCodeName((int)orderResponse.OrderStatusId);
                }

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
        public async Task UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);

                // Check to see if shipment exists
                if (shipment == null)
                {
                    continue;
                }

                if (!shipment.Order.IsManual || shipment.Order.CombineSplitStatus == CombineSplitStatusType.Combined)
                {
                    await webClient.AddShipment(store, shipment).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public async Task UpdateShipmentDetails(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            if (!shipment.Order.IsManual)
            {
                await webClient.AddShipment(store, shipment).ConfigureAwait(false);
            }
        }
    }
}