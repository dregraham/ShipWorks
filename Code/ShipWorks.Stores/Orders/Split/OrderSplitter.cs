using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitter(ISqlAdapterFactory sqlAdapterFactory, IOrderItemSplitter orderItemSplitter, 
            IOrderChargeSplitter orderChargeSplitter, IOrderSplitGateway orderSplitGateway)
        {
            this.orderSplitGateway = orderSplitGateway;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.orderItemSplitter = orderItemSplitter;
            this.orderChargeSplitter = orderChargeSplitter;
        }

        /// <summary>
        /// Split an order based on the definition
        /// </summary>
        public async Task<GenericResult<IDictionary<long, string>>> Split(OrderSplitDefinition definition)
        {
            Dictionary<long, string> resultOrderInfo = new Dictionary<long, string>();
            resultOrderInfo.Add(definition.Order.OrderID, definition.Order.OrderNumberComplete);

            OrderEntity originalOrder = await orderSplitGateway.LoadOrder(definition.Order.OrderID);

            if (originalOrder == null)
            {
                return GenericResult.FromError<IDictionary<long, string>>("Could not find surviving order");
            }

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

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
            {
                bool saveResult = true;
                saveResult &= await SaveOrder(newOrderEntity, sqlAdapter);
                saveResult &= await SaveOrder(originalOrder, sqlAdapter);

                if (!saveResult)
                {
                    sqlAdapter.Rollback();
                    return GenericResult.FromError<IDictionary<long, string>>("Saving split order failed");
                }

                sqlAdapter.Commit();
            }

            resultOrderInfo.Add(newOrderEntity.OrderID, newOrderEntity.OrderNumberComplete);

            return GenericResult.FromSuccess<IDictionary<long, string>>(resultOrderInfo);
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
        private static async Task<bool> SaveOrder(OrderEntity order, ISqlAdapter sqlAdapter)
        {
            order.OrderTotal = OrderUtility.CalculateTotal(order);

            bool saveResult = await sqlAdapter.SaveEntityAsync(order, true).ConfigureAwait(false);

            foreach (OrderItemEntity orderItem in order.OrderItems.Where(oi => Math.Abs(oi.Quantity) < 0.001))
            {
                // delete each attribute entity so derived entities are also deleted
                foreach (OrderItemAttributeEntity attrib in orderItem.OrderItemAttributes)
                {
                    saveResult &= await sqlAdapter.DeleteEntityAsync(attrib);
                }

                saveResult &= await sqlAdapter.DeleteEntityAsync(orderItem);
            }

            foreach (OrderChargeEntity orderCharge in order.OrderCharges.Where(oc => oc.Amount == 0))
            {
                saveResult &= await sqlAdapter.DeleteEntityAsync(orderCharge);
            }

            return saveResult;
        }

        /// <summary>
        /// Split the order values between the two orders
        /// </summary>
        private void SplitValues(OrderSplitDefinition definition, OrderEntity newOrderEntity, string newOrderNumber, OrderEntity originalOrder)
        {
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
