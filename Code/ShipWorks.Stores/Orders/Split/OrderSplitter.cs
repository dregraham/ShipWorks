using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Split.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

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
        private readonly IOrderItemSplitter orderItemSplitter;
        private readonly IOrderChargeSplitter orderChargeSplitter;
        private readonly IOrderChargeCalculator orderChargeCalculator;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitter(
            ISqlAdapterFactory sqlAdapterFactory,
            IOrderItemSplitter orderItemSplitter,
            IOrderChargeSplitter orderChargeSplitter,
            IOrderSplitGateway orderSplitGateway,
            IOrderChargeCalculator orderChargeCalculator)
        {
            this.orderSplitGateway = orderSplitGateway;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.orderItemSplitter = orderItemSplitter;
            this.orderChargeSplitter = orderChargeSplitter;
            this.orderChargeCalculator = orderChargeCalculator;
        }

        /// <summary>
        /// Split an order based on the definition
        /// </summary>
        public async Task<IDictionary<long, string>> Split(OrderSplitDefinition definition, IProgressReporter progressProvider)
        {
            return await orderSplitGateway
                .LoadOrder(definition.Order.OrderID)
                .Map(order => PerformSplit(order, definition))
                .Bind(x => SaveOrders(x.original, x.split, progressProvider))
                .Map(newOrder => new Dictionary<long, string>
                {
                    { definition.Order.OrderID, definition.Order.OrderNumberComplete },
                    { newOrder.OrderID, newOrder.OrderNumberComplete}
                })
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Perform the split
        /// </summary>
        private (OrderEntity original, OrderEntity split) PerformSplit(OrderEntity originalOrder, OrderSplitDefinition definition)
        {
            originalOrder.OrderCharges.RemovedEntitiesTracker = new EntityCollection<OrderChargeEntity>();
            originalOrder.OrderItems.RemovedEntitiesTracker = new EntityCollection<OrderItemEntity>();

            OrderEntity newOrderEntity = EntityUtility.CloneAsNew(definition.Order);
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
        /// Save the orders
        /// </summary>
        private async Task<OrderEntity> SaveOrders(OrderEntity originalOrder, OrderEntity newOrderEntity, IProgressReporter progressProvider)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
            {
                return await SaveOrder(newOrderEntity, sqlAdapter)
                    .Bind(x => SaveOrder(originalOrder, sqlAdapter).Map(y => x && y))
                    .Bind(x => CompleteTransaction(x, sqlAdapter, progressProvider))
                    .Map(_ => newOrderEntity)
                    .ConfigureAwait(false);
            }
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
        /// Add an order search to the new order
        /// </summary>
        private static void AddOrderSearch(OrderEntity newOrderEntity, OrderEntity originalOrder)
        {
            OrderSearchEntity orderSearch = newOrderEntity.OrderSearch.AddNew();
            orderSearch.OriginalOrderID = originalOrder.OrderID;
            orderSearch.IsManual = newOrderEntity.IsManual;
            orderSearch.OrderNumber = newOrderEntity.OrderNumber;
            orderSearch.OrderNumberComplete = newOrderEntity.OrderNumberComplete;
            orderSearch.StoreID = newOrderEntity.StoreID;
        }

        /// <summary>
        /// Save the values for the order to the database, including OrderItems 
        /// and OrderCharges (that may have been deleted)
        /// </summary>
        private async Task<bool> SaveOrder(OrderEntity order, ISqlAdapter sqlAdapter)
        {
            order.OrderTotal = orderChargeCalculator.CalculateTotal(order);

            bool saveResult = await sqlAdapter.SaveEntityAsync(order, true).ConfigureAwait(false);

            foreach (OrderItemEntity orderItem in order.OrderItems.Where(oi => Math.Abs(oi.Quantity) < 0.001))
            {
                // delete each attribute entity so derived entities are also deleted
                foreach (OrderItemAttributeEntity attrib in orderItem.OrderItemAttributes)
                {
                    saveResult &= await sqlAdapter.DeleteEntityAsync(attrib).ConfigureAwait(false);
                }

                saveResult &= await sqlAdapter.DeleteEntityAsync(orderItem).ConfigureAwait(false);
            }

            foreach (OrderChargeEntity orderCharge in order.OrderCharges.Where(oc => oc.Amount == 0))
            {
                saveResult &= await sqlAdapter.DeleteEntityAsync(orderCharge).ConfigureAwait(false);
            }

            return saveResult;
        }

        /// <summary>
        /// Split the order values between the two orders
        /// </summary>
        private void SplitValues(OrderSplitDefinition definition, OrderEntity newOrderEntity, string newOrderNumber, OrderEntity originalOrder)
        {
            originalOrder.CombineSplitStatus = CombineSplitStatusType.Split;

            newOrderEntity.IsNew = true;
            newOrderEntity.OrderID = 0;
            newOrderEntity.ChangeOrderNumber(newOrderNumber, "", "", newOrderEntity.OrderNumber);
            newOrderEntity.CombineSplitStatus = CombineSplitStatusType.Split;
            newOrderEntity.OnlineLastModified = originalOrder.OnlineLastModified;

            orderItemSplitter.Split(definition.ItemQuantities, originalOrder, newOrderEntity);

            orderChargeSplitter.Split(definition.ChargeAmounts, originalOrder, newOrderEntity);

            newOrderEntity.RollupItemCount = 0;
            newOrderEntity.RollupItemTotalWeight = 0;
            newOrderEntity.RollupNoteCount = 0;
        }
    }
}
