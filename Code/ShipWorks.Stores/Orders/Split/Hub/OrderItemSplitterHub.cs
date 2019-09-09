using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split.Hub
{
    /// <summary>
    /// Class for splitting order items between the original order and a new hub order
    /// </summary>
    public class OrderItemSplitterHub : IOrderDetailSplitterHub
    {
        /// <summary>
        /// Split the order items
        /// </summary>
        /// <param name="orderSplitDefinition">OrderSplitDefinition</param>
        /// <param name="originalOrder">The source order that is being split in two</param>
        public void Split(OrderSplitDefinition orderSplitDefinition, OrderEntity originalOrder)
        {
            // Go through each new order item quantities 
            foreach (KeyValuePair<long, decimal> orderItemQuantityDefinition in orderSplitDefinition.ItemQuantities)
            {
                //// Find the new order item based on OrderItemID
                //OrderItemEntity newOrderItemEntity = splitOrder.OrderItems.First(oi => oi.OrderItemID == orderItemQuantityDefinition.Key)

                // Find the original item based on OrderItemID
                OrderItemEntity originalOrderItemEntity = originalOrder.OrderItems.First(oi => oi.OrderItemID == orderItemQuantityDefinition.Key);

                // Update the new Quantity to be the split order defined Quantity
                double newQuantity = (double) orderItemQuantityDefinition.Value;

                // Update the original item quantity to be the difference of the two
                originalOrderItemEntity.Quantity = originalOrderItemEntity.Quantity - newQuantity;
            }

            //splitOrder.OrderItems
            //    .RemoveWhere(x => x.Quantity == 0 || !orderSplitDefinition.ItemQuantities.ContainsKey(x.OrderItemID))

            originalOrder.OrderItems
                .RemoveWhere(x => x.Quantity == 0 &&
                    orderSplitDefinition.ItemQuantities.TryGetValue(x.OrderItemID, out decimal splitQuantity) &&
                    splitQuantity != 0);
        }
    }
}
