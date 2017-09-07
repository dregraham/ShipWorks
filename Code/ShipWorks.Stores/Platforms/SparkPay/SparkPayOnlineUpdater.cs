using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Uploads shipment information to SparkPay
    /// </summary>
    [Component]
    public class SparkPayOnlineUpdater : ISparkPayOnlineUpdater
    {
        private readonly ISparkPayWebClient webClient;
        private readonly IOrderManager orderManager;
        private readonly Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory;
        private readonly ISparkPayCombineOrderSearchProvider combineOrderSearchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayOnlineUpdater(
            ISparkPayWebClient webClient,
            ISparkPayCombineOrderSearchProvider combineOrderSearchProvider,
            Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory,
            IOrderManager orderManager)
        {
            this.combineOrderSearchProvider = combineOrderSearchProvider;
            this.statusCodeProviderFactory = statusCodeProviderFactory;
            this.webClient = webClient;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Changes the status of an SparkPay order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(SparkPayStoreEntity store, long orderID, int statusCode)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOrderStatus(store, orderID, statusCode, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an SparkPay order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(SparkPayStoreEntity store, long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = orderManager.FetchOrder(orderID);

            if (order != null && (!order.IsManual || order.CombineSplitStatus == CombineSplitStatusType.Combined))
            {
                var statusCodeProvider = statusCodeProviderFactory(store);

                IEnumerable<long> orderNumbers = await combineOrderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);
                var handler = Result.Handle<SparkPayException>();

                var results = orderNumbers
                    .Select(x => handler.Execute(() => webClient.UpdateOrderStatus(store, x, statusCode)))
                    .ThrowFailures((msg, ex) => new SparkPayException(msg, ex))
                    .GetSuccessfulValues()
                    .ToList();

                var orderResponse = results.FirstOrDefault();

                order.OnlineStatusCode = orderResponse.OrderStatusId;
                if (orderResponse.OrderStatusId != null)
                {
                    order.OnlineStatus = statusCodeProvider.GetCodeName((int) orderResponse.OrderStatusId);
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
        /// Push the online status for an shipment.
        /// </summary>
        public async Task UpdateShipmentDetails(ISparkPayStoreEntity store, ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            var order = shipment.Order;

            IEnumerable<long> orderNumbers = await combineOrderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);
            var results = await UploadShipmentDetailsForCompleteOrder(store, shipment, orderNumbers);
            results.ThrowFailures((msg, ex) => new SparkPayException(msg, ex));
        }

        /// <summary>
        /// Upload the shipment details for all components of the order
        /// </summary>
        private async Task<IEnumerable<IResult>> UploadShipmentDetailsForCompleteOrder(ISparkPayStoreEntity store, ShipmentEntity shipment, IEnumerable<long> orderNumbers)
        {
            var handler = Result.Handle<SparkPayException>();
            var results = new List<IResult>();
            foreach (long orderNumber in orderNumbers)
            {
                var result = await handler.ExecuteAsync(() => webClient.AddShipment(store, shipment, orderNumber)).ConfigureAwait(false);
                results.Add(result);
            }

            return results;
        }
    }
}