﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.CombineOrderActions;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Combine multiple orders into a new, single order
    /// </summary>
    [Component]
    public class CombineOrder : ICombineOrder
    {
        private readonly IOrderManager orderManager;
        private readonly IDeletionService deletionService;
        private readonly IConfigurationData configurationData;
        private readonly IEnumerable<ICombineOrderAction> combinationActions;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrder(IOrderManager orderManager,
            IDeletionService deletionService,
            IConfigurationData configurationData,
            IEnumerable<ICombineOrderAction> combinationActions,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.combinationActions = combinationActions;
            this.configurationData = configurationData;
            this.deletionService = deletionService;
            this.orderManager = orderManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Perform the actual combination
        /// </summary>
        /// <remarks>
        /// I've purposely left the excessive spacing in this method to help extract each section into its own class
        /// </remarks>
        public async Task<GenericResult<long>> Combine(long survivingOrderID, IEnumerable<IOrderEntity> orders,
            string newOrderNumber, IProgressReporter progressReporter)
        {
            int totalCount = combinationActions.Count() + orders.Count() + 2;
            ProgressUpdater progress = new ProgressUpdater(progressReporter, totalCount);

            using (new AuditBehaviorScope(configurationData.FetchReadOnly().AuditDeletedOrders ? AuditState.Enabled : AuditState.NoDetails))
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
                {
                    return await PerformCombination(survivingOrderID, newOrderNumber, orders, progress, sqlAdapter);
                }
            }
        }

        /// <summary>
        /// Perform the actual combination
        /// </summary>
        private async Task<GenericResult<long>> PerformCombination(long survivingOrderID, string orderNumberComplete, IEnumerable<IOrderEntity> orders,
            ProgressUpdater progress, ISqlAdapter sqlAdapter)
        {
            OrderEntity combinedOrder = await CreateCombinedOrder(survivingOrderID, orderNumberComplete, orders, sqlAdapter);

            bool saveResult = await sqlAdapter.SaveEntityAsync(combinedOrder, true).ConfigureAwait(false);

            if (!saveResult)
            {
                sqlAdapter.Rollback();
                return GenericResult.FromError<long>("Save failed");
            }

            progress.Update();

            foreach (ICombineOrderAction action in combinationActions)
            {
                await action.Perform(combinedOrder, orders, sqlAdapter).ConfigureAwait(false);
                progress.Update();
            }

            DeleteOriginalOrders(orders, progress);

            await sqlAdapter.SaveEntityAsync(combinedOrder).ConfigureAwait(false);

            sqlAdapter.Commit();
            progress.Update();

            return GenericResult.FromSuccess(combinedOrder.OrderID);
        }

        /// <summary>
        /// Create the combined order from the surviving order id
        /// </summary>
        private async Task<OrderEntity> CreateCombinedOrder(long survivingOrderID, string orderNumberComplete, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            OrderEntity combinedOrder = await orderManager.LoadOrderAsync(survivingOrderID, sqlAdapter).ConfigureAwait(false);

            combinedOrder.IsNew = true;
            combinedOrder.OrderID = 0;
            combinedOrder.OrderNumberComplete = orderNumberComplete;
            combinedOrder.CombineSplitStatus = CombineSplitStatusType.Combined;
            combinedOrder.OnlineLastModified = orders.Max(x => x.OnlineLastModified);
            combinedOrder.RollupItemCount = 0;
            combinedOrder.RollupItemTotalWeight = 0;
            combinedOrder.RollupNoteCount = 0;

            foreach (IEntityFieldCore field in combinedOrder.Fields)
            {
                field.IsChanged = true;
            }

            return combinedOrder;
        }

        /// <summary>
        /// Delete the original orders
        /// </summary>
        private void DeleteOriginalOrders(IEnumerable<IOrderEntity> orders, ProgressUpdater progress)
        {
            foreach (IOrderEntity order in orders)
            {
                deletionService.DeleteOrder(order.OrderID);
                progress.Update();
            }
        }
    }
}
