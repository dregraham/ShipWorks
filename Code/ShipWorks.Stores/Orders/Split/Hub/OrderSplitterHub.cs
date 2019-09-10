using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Orders.Split.Errors;
using ShipWorks.Warehouse;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.Stores.Orders.Split.Hub
{
    /// <summary>
    /// Split an order
    /// </summary>
    [KeyedComponent(typeof(IOrderSplitter), OrderSplitterType.Reroute)]
    public class OrderSplitterHub : IOrderSplitter
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IOrderSplitGateway orderSplitGateway;
        private readonly IEnumerable<IOrderDetailSplitterHub> orderDetailSplitters;
        private readonly IOrderSplitAuditHub splitOrderAudit;
        private readonly IConfigurationData configurationData;
        private readonly IWarehouseOrderClient warehouseOrderClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitterHub(
            ISqlAdapterFactory sqlAdapterFactory,
            IEnumerable<IOrderDetailSplitterHub> orderDetailSplitters,
            IOrderSplitGateway orderSplitGateway,
            IOrderSplitAuditHub splitOrderAudit,
            IConfigurationData configurationData,
            IWarehouseOrderClient warehouseOrderClient
            )
        {
            this.orderSplitGateway = orderSplitGateway;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.orderDetailSplitters = orderDetailSplitters;
            this.splitOrderAudit = splitOrderAudit;
            this.configurationData = configurationData;
            this.warehouseOrderClient = warehouseOrderClient;
        }

        /// <summary>
        /// Split an order based on the definition
        /// </summary>
        public Task<IDictionary<long, string>> Split(OrderSplitDefinition definition, IProgressReporter progressProvider)
        {
            return UsingAsync(
                new TrackedDurationEvent("OrderManagement.Orders.Split"),
                evt => SplitInternal(definition, progressProvider)
                    .Do(_ => AddTelemetryProperties(evt, definition),
                        ex => AddTelemetryProperties(evt, definition, ex)));
        }

        /// <summary>
        /// Split an order based on the definition
        /// </summary>
        private Task<IDictionary<long, string>> SplitInternal(OrderSplitDefinition definition, IProgressReporter progressProvider)
        {
            return orderSplitGateway
                .LoadOrder(definition.Order.OrderID)
                .Bind(order => PerformSplit(order, definition))
                .Bind(x => SaveOrders(x, progressProvider))
                .Bind(AuditOrders)
                .Map(originalOrder => new[] { definition.Order }.ToDictionary(x => x.OrderID, x => x.OrderNumberComplete))
                .Map(x => x as IDictionary<long, string>);
        }

        /// <summary>
        /// Add telemetry properties
        /// </summary>
        private void AddTelemetryProperties(TrackedDurationEvent trackedDurationEvent, OrderSplitDefinition definition, Exception ex = null)
        {
            try
            {
                OrderEntity order = definition?.Order;

                if (order != null)
                {
                    trackedDurationEvent.AddProperty("Orders.Split.Result", ex == null ? "Success" : "Failed");
                    trackedDurationEvent.AddProperty("Orders.Split.PreSplitStatus", EnumHelper.GetDescription(order.CombineSplitStatus));
                    trackedDurationEvent.AddProperty("Orders.Split.StoreType", EnumHelper.GetDescription(order.Store.StoreTypeCode));
                    trackedDurationEvent.AddProperty("Orders.Split.StoreId", order.StoreID.ToString());
                    trackedDurationEvent.AddProperty("Orders.Split.OriginalOrder", order.OrderNumberComplete);
                    trackedDurationEvent.AddProperty("Orders.Split.Type", EnumHelper.GetDescription(definition.OrderSplitterType));

                    int originalItemCount = order.OrderItems.Count;
                    int splitOrderItemCount = definition.ItemQuantities.Values.Count(q => q > 0);

                    trackedDurationEvent.AddProperty("Orders.Split.NewOrder.ItemCount", splitOrderItemCount.ToString());
                    trackedDurationEvent.AddProperty("Orders.Split.OriginalOrder.ItemCount", (originalItemCount - splitOrderItemCount).ToString());

                    if (ex != null)
                    {
                        trackedDurationEvent.AddProperty("Orders.Split.Reroute.FailureReason", ex.Message);
                    }
                }
            }
            catch
            {
                // Just continue...we don't want to stop the combine if telemetry has an issue.
            }
        }

        /// <summary>
        /// Perform the split
        /// </summary>
        private async Task<OrderEntity> PerformSplit(OrderEntity originalOrder, OrderSplitDefinition definition)
        {
            originalOrder.OrderCharges.RemovedEntitiesTracker = new EntityCollection<OrderChargeEntity>();
            originalOrder.OrderItems.RemovedEntitiesTracker = new EntityCollection<OrderItemEntity>();

            SplitValues(definition, definition.NewOrderNumber, originalOrder);

            var itemValues = definition.Order.OrderItems.ToDictionary(oi => oi.OrderItemID, oi => oi.HubItemID);

            ItemsToReroute itemsToReroute = new ItemsToReroute()
            {
                    FromWarehouseId = new Guid(configurationData.FetchReadOnly().WarehouseID).ToString("N"),
                    Items = definition.ItemQuantities
                        .Where(iq => iq.Value > 0)
                        .Select(iq => 
                        new ItemQuantity()
                        {
                            Id = itemValues[iq.Key],
                            Quantity = iq.Value
                        })
            };

            await warehouseOrderClient.RerouteOrderItems(originalOrder.HubOrderID.Value, itemsToReroute)
                .ConfigureAwait(false);

            return originalOrder;
        }

        /// <summary>
        /// Audit each order
        /// </summary>
        private async Task<OrderEntity> AuditOrders(OrderEntity originalOrder)
        {
            await splitOrderAudit.Audit(originalOrder).ConfigureAwait(false);

            return originalOrder;
        }

        /// <summary>
        /// Save the orders
        /// </summary>
        private async Task<OrderEntity> SaveOrders(OrderEntity originalOrder, IProgressReporter progressProvider)
        {
            return await sqlAdapterFactory.WithPhysicalTransactionAsync(async sqlAdapter =>
            {
                await SaveOrder(originalOrder, sqlAdapter)
                    .Bind(x => CompleteTransaction(x, sqlAdapter, progressProvider))
                    .ConfigureAwait(false);

                return originalOrder;
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Complete the transaction
        /// </summary>
        private Task<Unit> CompleteTransaction(bool saveSucceeded, ISqlAdapter sqlAdapter, IProgressReporter progressProvider)
        {
            var cancelRequested = progressProvider.IsCancelRequested;
            progressProvider.CanCancel = false;

            if (!saveSucceeded || cancelRequested)
            {
                sqlAdapter.Rollback();
                return Result.FromError(cancelRequested ? Error.Canceled : Error.SaveFailed);
            }

            progressProvider.PercentComplete = 50;

            sqlAdapter.Commit();
            return Result.FromSuccess();
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
        private void SplitValues(OrderSplitDefinition definition, string newOrderNumber, OrderEntity originalOrder)
        {
            originalOrder.CombineSplitStatus = originalOrder.CombineSplitStatus.AsSplit();

            foreach (IOrderDetailSplitterHub orderDetailSplitter in orderDetailSplitters)
            {
                orderDetailSplitter.Split(definition, originalOrder);
            }
        }
    }
}
