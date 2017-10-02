using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Updates BigCommerce order status/shipments
    /// </summary>
    [Component]
    public class BigCommerceOrderStatusUpdater : IBigCommerceOrderStatusUpdater
    {
        private readonly ILog log;
        private readonly IBigCommerceWebClientFactory webClientFactory;
        private readonly Func<BigCommerceStoreEntity, IBigCommerceStatusCodeProvider> createStatusCodeProvider;
        private readonly IBigCommerceDataAccess dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceOrderStatusUpdater(
            IBigCommerceDataAccess dataAccess,
            IBigCommerceWebClientFactory webClientFactory,
            Func<BigCommerceStoreEntity, IBigCommerceStatusCodeProvider> createStatusCodeProvider,
            Func<Type, ILog> createLogger)
        {
            this.dataAccess = dataAccess;
            this.createStatusCodeProvider = createStatusCodeProvider;
            this.webClientFactory = webClientFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(BigCommerceStoreEntity store, long orderID, int statusCode)
        {
            IUnitOfWorkCore unitOfWork = dataAccess.GetUnitOfWork();
            await UpdateOrderStatus(store, orderID, statusCode, unitOfWork).ConfigureAwait(false);
            await dataAccess.Commit(unitOfWork).ConfigureAwait(false);
        }

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(BigCommerceStoreEntity store, long orderID, int statusCode, IUnitOfWorkCore unitOfWork)
        {
            var orderDetails = await dataAccess.GetOrderDetailsAsync(orderID).ConfigureAwait(false);

            if (orderDetails == null)
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
                return;
            }

            await UpdateOrderStatus(orderDetails, statusCode, store, unitOfWork).ConfigureAwait(false);
        }

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(BigCommerceStoreEntity store, BigCommerceOnlineOrder orderDetails, int statusCode)
        {
            IUnitOfWorkCore unitOfWork = dataAccess.GetUnitOfWork();
            await UpdateOrderStatus(orderDetails, statusCode, store, unitOfWork).ConfigureAwait(false);
            await dataAccess.Commit(unitOfWork).ConfigureAwait(false);
        }

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        private async Task UpdateOrderStatus(BigCommerceOnlineOrder orderDetails, int statusCode, BigCommerceStoreEntity store, IUnitOfWorkCore unitOfWork)
        {
            if (orderDetails.AreAllManual)
            {
                log.InfoFormat("Not uploading order status since order {0} is manual or the order only has digital items.", orderDetails.OrderID);
                return;
            }

            IBigCommerceWebClient webClient = webClientFactory.Create(store);

            List<IResult> allResults = new List<IResult>();
            foreach (var chunk in orderDetails.OrdersToUpload.SplitIntoChunksOf(4))
            {
                var tasks = chunk.Select(x => Result.Handle<BigCommerceException>().ExecuteAsync(() => webClient.UpdateOrderStatus(Convert.ToInt32(x.OrderNumber), statusCode)));
                var results = await Task.WhenAll(tasks).ConfigureAwait(false);
                allResults.AddRange(results);
            }

            allResults.ThrowFailures((msg, ex) => new BigCommerceException(msg, ex));

            IBigCommerceStatusCodeProvider statusCodeProvider = createStatusCodeProvider(store);

            // Update the local database with the new status
            OrderEntity basePrototype = new OrderEntity(orderDetails.OrderID) { IsNew = false };
            basePrototype.OnlineStatusCode = statusCode;
            basePrototype.OnlineStatus = statusCodeProvider.GetCodeName(statusCode);

            unitOfWork.AddForSave(basePrototype);
        }
    }
}
