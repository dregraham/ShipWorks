using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// Status updater for NetworkSolutions orders
    /// </summary>
    [Component]
    public class OrderStatusUpdater : IOrderStatusUpdater
    {
        private readonly Func<NetworkSolutionsStoreEntity, NetworkSolutionsStatusCodeProvider> createStatusCodeProvider;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IDataProvider dataProvider;
        private readonly ITemplateTokenProcessor templateTokenProcessor;
        private readonly ILog log;
        private readonly INetworkSolutionsCombineOrderSearchProvider orderSearchProvider;
        private readonly INetworkSolutionsWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="templateTokenProcessor"></param>
        /// <param name="sqlAdapterFactory"></param>
        [NDependIgnoreTooManyParams]
        public OrderStatusUpdater(Func<NetworkSolutionsStoreEntity, NetworkSolutionsStatusCodeProvider> createStatusCodeProvider,
            INetworkSolutionsCombineOrderSearchProvider orderSearchProvider,
            IDataProvider dataProvider,
            ITemplateTokenProcessor templateTokenProcessor,
            INetworkSolutionsWebClient webClient,
            ISqlAdapterFactory sqlAdapterFactory,
            Func<Type, ILog> createLogger)
        {
            this.webClient = webClient;
            this.orderSearchProvider = orderSearchProvider;
            this.createStatusCodeProvider = createStatusCodeProvider;
            this.templateTokenProcessor = templateTokenProcessor;
            this.dataProvider = dataProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Changes the status of an NetworkSolutions order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(NetworkSolutionsStoreEntity store, long orderID, long statusCode, string comments)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOrderStatus(store, orderID, statusCode, comments, unitOfWork).ConfigureAwait(false);

            using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
            {
                await unitOfWork.CommitAsync(adapter.AsDataAccessAdapter()).ConfigureAwait(false);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an NetworkSolutions order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(NetworkSolutionsStoreEntity store, long orderID, long statusCode, string comments, UnitOfWork2 unitOfWork)
        {
            NetworkSolutionsOrderEntity order = await dataProvider.GetEntityAsync<NetworkSolutionsOrderEntity>(orderID);
            if (order == null)
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
                return;
            }

            if (order.IsManual && order.CombineSplitStatus != CombineSplitStatusType.Combined)
            {
                log.WarnFormat("Not uploading order status since order {0} is manual.", order.OrderID);
                return;
            }

            string processedComment = templateTokenProcessor.ProcessTokens(comments, orderID);

            var exceptions = new List<Exception>();
            var identifiers = await orderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            foreach (var identifier in identifiers)
            {
                try
                {
                    await Task.Run(() =>
                        webClient.UpdateOrderStatus(store, identifier, (long) order.OnlineStatusCode, statusCode, processedComment));
                }
                catch (NetworkSolutionsException ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw exceptions.First();
            }

            // Update the local database with the new status
            OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
            basePrototype.OnlineStatusCode = statusCode;
            basePrototype.OnlineStatus = createStatusCodeProvider(store).GetCodeName(statusCode);

            unitOfWork.AddForSave(basePrototype);
        }
    }
}
