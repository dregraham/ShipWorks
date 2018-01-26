using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Orders.Split.Actions;
using ShipWorks.Stores.Orders.Split.Errors;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Split an order
    /// </summary>
    [Component]
    public class OrderSplitter : IOrderSplitter
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IOrderSplitGateway orderSplitGateway;
        private readonly IEnumerable<IOrderDetailSplitter> orderDetailSplitters;
        private readonly IOrderSplitAudit splitOrderAudit;
        private CombineSplitStatusType originalOrderCombineSplitStatus = CombineSplitStatusType.None;
        private readonly IIndex<StoreTypeCode, IStoreSpecificSplitOrderAction> storeSpecificOrderSplitter;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitter(
            ISqlAdapterFactory sqlAdapterFactory,
            IEnumerable<IOrderDetailSplitter> orderDetailSplitters,
            IOrderSplitGateway orderSplitGateway,
            IOrderSplitAudit splitOrderAudit,
            IIndex<StoreTypeCode, IStoreSpecificSplitOrderAction> storeSpecificOrderSplitter
            )
        {
            this.orderSplitGateway = orderSplitGateway;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.orderDetailSplitters = orderDetailSplitters;
            this.splitOrderAudit = splitOrderAudit;
            this.storeSpecificOrderSplitter = storeSpecificOrderSplitter;
        }

        /// <summary>
        /// Split an order based on the definition
        /// </summary>
        public Task<IDictionary<long, string>> Split(OrderSplitDefinition definition, IProgressReporter progressProvider)
        {
            return UsingAsync(
                new TrackedDurationEvent("OrderManagement.Orders.Split"),
                evt => SplitInternal(definition, progressProvider)
                    .Do(_ => AddTelemetryProperties(evt, definition, true),
                        _ => AddTelemetryProperties(evt, definition, false)));
        }

        /// <summary>
        /// Split an order based on the definition
        /// </summary>
        private Task<IDictionary<long, string>> SplitInternal(OrderSplitDefinition definition, IProgressReporter progressProvider)
        {
            return orderSplitGateway
                .LoadOrder(definition.Order.OrderID)
                .Map(order => PerformSplit(order, definition))
                .Bind(x => SaveOrders(x.original, x.split, progressProvider))
                .Bind(x => AuditOrders(x.original, x.split))
                .Map(newOrder => new[] { definition.Order, newOrder.split }.ToDictionary(x => x.OrderID, x => x.OrderNumberComplete))
                .Map(x => x as IDictionary<long, string>);
        }

        /// <summary>
        /// Add telemetry properties
        /// </summary>
        private void AddTelemetryProperties(TrackedDurationEvent trackedDurationEvent, OrderSplitDefinition definition, bool result)
        {
            var order = definition?.Order;

            try
            {
                trackedDurationEvent.AddProperty("Orders.Split.Result", result ? "Success" : "Failed");
                trackedDurationEvent.AddProperty("Orders.Split.PreSplitStatus", EnumHelper.GetDescription(order.CombineSplitStatus));
                trackedDurationEvent.AddProperty("Orders.Split.StoreType", EnumHelper.GetDescription(order.Store.StoreTypeCode));
                trackedDurationEvent.AddProperty("Orders.Split.StoreId", order.StoreID.ToString());
                trackedDurationEvent.AddProperty("Orders.Split.OriginalOrder", order.OrderNumberComplete);
            }
            catch
            {
                // Just continue...we don't want to stop the combine if telemetry has an issue.
            }
        }

        /// <summary>
        /// Perform the split
        /// </summary>
        private (OrderEntity original, OrderEntity split) PerformSplit(OrderEntity originalOrder, OrderSplitDefinition definition)
        {
            originalOrderCombineSplitStatus = originalOrder.CombineSplitStatus;

            originalOrder.OrderCharges.RemovedEntitiesTracker = new EntityCollection<OrderChargeEntity>();
            originalOrder.OrderItems.RemovedEntitiesTracker = new EntityCollection<OrderItemEntity>();

            OrderEntity newOrderEntity = EntityUtility.CloneAsNew(originalOrder);
            newOrderEntity.OrderCharges.RemovedEntitiesTracker = new EntityCollection<OrderChargeEntity>();
            newOrderEntity.OrderItems.RemovedEntitiesTracker = new EntityCollection<OrderItemEntity>();

            SplitValues(definition, newOrderEntity, definition.NewOrderNumber, originalOrder);

            AddOrderSearch(newOrderEntity, originalOrder);

            foreach (IEntityFieldCore field in newOrderEntity.Fields)
            {
                field.IsChanged = true;
            }

            return (originalOrder, newOrderEntity);
        }

        /// <summary>
        /// Audit each order
        /// </summary>
        private async Task<(OrderEntity original, OrderEntity split)> AuditOrders(OrderEntity originalOrder, OrderEntity newOrderEntity)
        {
            await splitOrderAudit.Audit(originalOrder, newOrderEntity).ConfigureAwait(false);

            return (originalOrder, newOrderEntity);
        }

        /// <summary>
        /// Save the orders
        /// </summary>
        private async Task<(OrderEntity original, OrderEntity split)> SaveOrders(OrderEntity originalOrder, OrderEntity newOrderEntity, IProgressReporter progressProvider)
        {
            return await sqlAdapterFactory.WithPhysicalTransactionAsync(async (transaction, sqlAdapter) =>
            {
                await SaveOrder(newOrderEntity, sqlAdapter)
                    .Bind(x => SaveOrder(originalOrder, sqlAdapter).Map(y => x && y))
                    .Bind(x => CompleteTransaction(x, transaction, sqlAdapter, progressProvider))
                    .Map(_ => newOrderEntity)
                    .ConfigureAwait(false);

                return (originalOrder, newOrderEntity);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Complete the transaction
        /// </summary>
        private Task<Unit> CompleteTransaction(bool saveSucceeded, DbTransaction transaction, ISqlAdapter sqlAdapter, IProgressReporter progressProvider)
        {
            var cancelRequested = progressProvider.IsCancelRequested;
            progressProvider.CanCancel = false;

            if (!saveSucceeded || cancelRequested)
            {
                transaction.Rollback();
                return Result.FromError(cancelRequested ? Error.Canceled : Error.SaveFailed);
            }

            progressProvider.PercentComplete = 50;

            transaction.Commit();
            return Result.FromSuccess();
        }

        /// <summary>
        /// Add an order search to the new order
        /// </summary>
        private void AddOrderSearch(OrderEntity newOrderEntity, OrderEntity originalOrder)
        {
            if (originalOrderCombineSplitStatus == CombineSplitStatusType.None)
            {
                // Create an OrderSearch for the new order.  So that we get back to original order,
                // everything is the same except for the OrderID.
                OrderSearchEntity newOrderSearch = newOrderEntity.OrderSearch.AddNew();
                newOrderSearch.OriginalOrderID = originalOrder.OrderID;
                newOrderSearch.IsManual = originalOrder.IsManual;
                newOrderSearch.OrderNumber = originalOrder.OrderNumber;
                newOrderSearch.OrderNumberComplete = originalOrder.OrderNumberComplete;
                newOrderSearch.StoreID = originalOrder.StoreID;

                // Also create an OrderSearch for the original order so that we know that
                // it was part of a split operation and searching works.
                OrderSearchEntity orderSearch = originalOrder.OrderSearch.AddNew();
                orderSearch.OriginalOrderID = originalOrder.OrderID;
                orderSearch.IsManual = originalOrder.IsManual;
                orderSearch.OrderNumber = originalOrder.OrderNumber;
                orderSearch.OrderNumberComplete = originalOrder.OrderNumberComplete;
                orderSearch.StoreID = originalOrder.StoreID;

                // Add store specific search entries.
                IStoreSpecificSplitOrderAction platformSplitter;
                storeSpecificOrderSplitter.TryGetValue((StoreTypeCode) originalOrder.Store.TypeCode, out platformSplitter);

                platformSplitter?.Perform(originalOrder.OrderID, newOrderEntity);
                platformSplitter?.Perform(originalOrder.OrderID, originalOrder);
            }
        }

        /// <summary>
        /// Save the values for the order to the database, including OrderItems 
        /// and OrderCharges (that may have been deleted)
        /// </summary>
        private async Task<bool> SaveOrder(OrderEntity order, ISqlAdapter sqlAdapter)
        {
            order.OrderTotal = OrderUtility.CalculateTotal(order);

            bool saveResult = await sqlAdapter.SaveEntityAsync(order, true).ConfigureAwait(false);

            foreach (OrderItemEntity orderItem in order.OrderItems.RemovedEntitiesTracker)
            {
                saveResult &= await DeleteCollection(sqlAdapter, orderItem.OrderItemAttributes).ConfigureAwait(false);
            }

            saveResult &= await DeleteCollection(sqlAdapter, order.OrderItems.RemovedEntitiesTracker).ConfigureAwait(false);
            saveResult &= await DeleteCollection(sqlAdapter, order.OrderCharges.RemovedEntitiesTracker).ConfigureAwait(false);

            return saveResult;
        }

        /// <summary>
        /// Delete a collection
        /// </summary>
        private async Task<bool> DeleteCollection(ISqlAdapter sqlAdapter, IEntityCollection2 collection)
        {
            var shouldBeDeleted = collection.OfType<IEntityCore>().Count(x => !x.IsNew);

            if (shouldBeDeleted == 0)
            {
                return true;
            }

            var deletedCount = await sqlAdapter.DeleteEntityCollectionAsync(collection).ConfigureAwait(false);
            return deletedCount == shouldBeDeleted;
        }

        /// <summary>
        /// Split the order values between the two orders
        /// </summary>
        private void SplitValues(OrderSplitDefinition definition, OrderEntity newOrderEntity, string newOrderNumber, OrderEntity originalOrder)
        {
            originalOrder.CombineSplitStatus = originalOrder.CombineSplitStatus.AsSplit();

            newOrderEntity.IsNew = true;
            newOrderEntity.OrderID = 0;
            newOrderEntity.ChangeOrderNumber(newOrderNumber, "", "", newOrderEntity.OrderNumber);

            newOrderEntity.CombineSplitStatus = newOrderEntity.CombineSplitStatus.AsSplit();

            newOrderEntity.OnlineLastModified = originalOrder.OnlineLastModified;

            foreach (IOrderDetailSplitter orderDetailSplitter in orderDetailSplitters)
            {
                orderDetailSplitter.Split(definition, originalOrder, newOrderEntity);
            }

            newOrderEntity.RollupItemCount = 0;
            newOrderEntity.RollupItemTotalWeight = 0;
            newOrderEntity.RollupNoteCount = 0;
        }
    }
}
