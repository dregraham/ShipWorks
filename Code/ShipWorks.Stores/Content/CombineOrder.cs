using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.CombineOrderActions;
using ShipWorks.Users.Audit;
using System;

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
        private readonly ICombineOrderAudit combinedOrderAuditor;
        private readonly IStoreTypeManager storeTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public CombineOrder(IOrderManager orderManager,
            IDeletionService deletionService,
            IConfigurationData configurationData,
            IEnumerable<ICombineOrderAction> combinationActions,
            ISqlAdapterFactory sqlAdapterFactory,
            ICombineOrderAudit combinedOrderAuditor,
            IStoreTypeManager storeTypeManager)
        {
            this.combinationActions = combinationActions;
            this.configurationData = configurationData;
            this.deletionService = deletionService;
            this.orderManager = orderManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.combinedOrderAuditor = combinedOrderAuditor;
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Perform the actual combination
        /// </summary>
        /// <remarks>
        /// I've purposely left the excessive spacing in this method to help extract each section into its own class
        /// </remarks>
        public Task<GenericResult<long>> Combine(long survivingOrderID, IEnumerable<IOrderEntity> orders, string newOrderNumber) =>
            Combine(survivingOrderID, orders, newOrderNumber, ProgressUpdater.Empty);

        /// <summary>
        /// Perform the actual combination
        /// </summary>
        /// <remarks>
        /// I've purposely left the excessive spacing in this method to help extract each section into its own class
        /// </remarks>
        public Task<GenericResult<long>> Combine(long survivingOrderID, IEnumerable<IOrderEntity> orders,
            string newOrderNumber, IProgressReporter progressReporter)
        {
            int totalCount = combinationActions.Count() + orders.Count() + 2;
            ProgressUpdater progress = new ProgressUpdater(progressReporter, totalCount);

            return Combine(survivingOrderID, orders, newOrderNumber, progress);
        }

        /// <summary>
        /// Perform the actual combination
        /// </summary>
        /// <remarks>
        /// I've purposely left the excessive spacing in this method to help extract each section into its own class
        /// </remarks>
        private async Task<GenericResult<long>> Combine(long survivingOrderID, IEnumerable<IOrderEntity> orders,
            string newOrderNumber, IProgressUpdater progress)
        {
            GenericResult<long> result;

            using (new AuditBehaviorScope(configurationData.FetchReadOnly().AuditDeletedOrders ? AuditState.Enabled : AuditState.NoDetails))
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
                {
                    SqlAdapterRetry<SqlException> sqlDeadlockRetry = new SqlAdapterRetry<SqlException>(5, -5, string.Format("CombineOrder.Combine for new order number {0}", newOrderNumber));
                    result = await sqlDeadlockRetry.ExecuteWithRetryAsync(() => PerformCombination(survivingOrderID, newOrderNumber, orders, progress, sqlAdapter)).ConfigureAwait(false);
                }
            }

            if (result.Success)
            {
                await combinedOrderAuditor.Audit(result.Value, orders);
            }

            return result;
        }

        /// <summary>
        /// Perform the actual combination
        /// </summary>
        private async Task<GenericResult<long>> PerformCombination(long survivingOrderID, string orderNumberComplete, IEnumerable<IOrderEntity> orders,
            IProgressUpdater progress, ISqlAdapter sqlAdapter)
        {
            GenericResult<long> existingOrdersResult = await EnsureOrdersHaveNotChanged(orders, sqlAdapter).ConfigureAwait(false);
            if (existingOrdersResult.Failure)
            {
                return existingOrdersResult;
            }

            GenericResult<OrderEntity> combinedOrderResult = await CreateCombinedOrder(survivingOrderID, orderNumberComplete, orders, sqlAdapter).ConfigureAwait(false);
            if (combinedOrderResult.Failure)
            {
                return GenericResult.FromError<long>(combinedOrderResult.Message);
            }

            OrderEntity combinedOrder = combinedOrderResult.Value;
            bool saveResult = await sqlAdapter.SaveEntityAsync(combinedOrder, true).ConfigureAwait(false);

            if (!saveResult)
            {
                sqlAdapter.Rollback();
                return GenericResult.FromError<long>("Save failed");
            }

            progress.Update();

            foreach (ICombineOrderAction action in combinationActions)
            {
                await action.Perform(combinedOrder, survivingOrderID, orders, sqlAdapter).ConfigureAwait(false);
                progress.Update();
            }

            DeleteOriginalOrders(orders, sqlAdapter, progress);

            await sqlAdapter.SaveEntityAsync(combinedOrder).ConfigureAwait(false);

            sqlAdapter.Commit();

            progress.Update();

            return GenericResult.FromSuccess(combinedOrder.OrderID);
        }

        /// <summary>
        /// Ensure that the orders to be combined have not changed
        /// </summary>
        private async Task<GenericResult<long>> EnsureOrdersHaveNotChanged(IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<OrderEntity> updatedOrders = await orderManager.LoadOrdersAsync(orders.Select(x => x.OrderID), sqlAdapter).ConfigureAwait(false);
            var mergedOrders = orders.LeftJoin(updatedOrders, x => x.OrderID, x => x.OrderID);

            var deletedOrders = mergedOrders.Where(x => x.Item2 == null);
            if (deletedOrders.Any())
            {
                return GenericResult.FromError<long>("Some orders were deleted");
            }

            var changedOrders = mergedOrders.Where(x => x.Item1.RowVersion.Except(x.Item2.RowVersion).Any());
            if (changedOrders.Any())
            {
                return GenericResult.FromError<long>("Some orders were changed");
            }

            return GenericResult.FromSuccess(0L);
        }

        /// <summary>
        /// Create the combined order from the surviving order id
        /// </summary>
        private async Task<GenericResult<OrderEntity>> CreateCombinedOrder(long survivingOrderID, string orderNumberComplete, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            OrderEntity combinedOrder = await orderManager.LoadOrderAsync(survivingOrderID, sqlAdapter).ConfigureAwait(false);

            if (combinedOrder == null)
            {
                return GenericResult.FromError<OrderEntity>("Could not find surviving order");
            }

            if (combinedOrder.IsManual)
            {
                combinedOrder = CreateCombinedOrderForManualSurvivingOrder(combinedOrder, orders);
            }

            // Default to now, assuming all orders are manual
            DateTime onlineLastModified = DateTime.UtcNow;

            // If we have at least one order that is not manual, grab the max last modified
            if (orders.Any(o => !o.IsManual))
            {
                onlineLastModified = orders.Where(o => !o.IsManual).Max(x => x.OnlineLastModified);
            }

            combinedOrder.IsNew = true;
            combinedOrder.OrderID = 0;
            combinedOrder.ChangeOrderNumber(orderNumberComplete, string.Empty, string.Empty, combinedOrder.OrderNumber);
            combinedOrder.CombineSplitStatus = CombineSplitStatusType.Combined;
            combinedOrder.OnlineLastModified = onlineLastModified;
            combinedOrder.RollupItemCount = 0;
            combinedOrder.RollupItemTotalWeight = 0;
            combinedOrder.RollupNoteCount = 0;
            combinedOrder.OrderTotal = orders.Sum(o => o.OrderTotal);
            combinedOrder.IsManual = orders.All(o => o.IsManual);

            foreach (IEntityFieldCore field in combinedOrder.Fields)
            {
                field.IsChanged = true;
            }

            return GenericResult.FromSuccess(combinedOrder);
        }

        /// <summary>
        /// If the order is manual, convert it to an actual store specific order type.
        /// </summary>
        private OrderEntity CreateCombinedOrderForManualSurvivingOrder(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders)
        {
            if (!combinedOrder.IsManual)
            {
                return combinedOrder;
            }

            StoreType storeType = storeTypeManager.GetType(combinedOrder.StoreID);
            OrderEntity convertedOrder = storeType.CreateOrder();

            convertedOrder.InitializeNullsToDefault();

            foreach (IEntityFieldCore field in combinedOrder.Fields.Where(f => !f.IsReadOnly))
            {
                convertedOrder.Fields[field.FieldIndex].CurrentValue = field.CurrentValue;
            }

            if (orders.Any(o => !o.IsManual))
            {
                convertedOrder.OrderDate = orders.Where(o => !o.IsManual).Max(x => x.OrderDate);
            }

            return convertedOrder;
        }

        /// <summary>
        /// Delete the original orders
        /// </summary>
        private void DeleteOriginalOrders(IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter, IProgressUpdater progress)
        {
            foreach (IOrderEntity order in orders)
            {
                deletionService.DeleteOrder(order.OrderID, sqlAdapter);
                progress.Update();
            }
        }
    }
}
