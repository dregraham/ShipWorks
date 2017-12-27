using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Class for splitting order items between two orders
    /// </summary>
    [Component]
    public class OrderItemSplitter : IOrderItemSplitter
    {
        /// <summary>
        /// Split the order items
        /// </summary>
        /// <param name="newOrderItemQuantities">Dictionary<OrderItemID, Quantity></param>
        /// <param name="originalOrder">The source order that is being split in two</param>
        /// <param name="splitOrder">The new order created from originalOrder</param>
        public void Split(IDictionary<long, double> newOrderItemQuantities, OrderEntity originalOrder, OrderEntity splitOrder)
        {
            // Go through each new order item quantities 
            foreach (KeyValuePair<long, double> orderItemQuantityDefinition in newOrderItemQuantities)
            {
                // Find the new order item based on OrderItemID
                OrderItemEntity newOrderItemEntity = splitOrder.OrderItems.First(oi => oi.OrderItemID == orderItemQuantityDefinition.Key);

                // Find the original item based on OrderItemID
                OrderItemEntity originalOrderItemEntity = originalOrder.OrderItems.First(oi => oi.OrderItemID == orderItemQuantityDefinition.Key);

                // Update the new Quantity to be the split order defined Quantity
                newOrderItemEntity.Quantity = orderItemQuantityDefinition.Value;

                // Update the original item quantity to be the difference of the two
                originalOrderItemEntity.Quantity = originalOrderItemEntity.Quantity - newOrderItemEntity.Quantity;
            }

            // Remove any order items from the split order that were not in the newOrderItemQuantities
            // or had 0 Quantity
            IList<long> notPresentOrderItemIDs = splitOrder.OrderItems
                .Select(oi => oi.OrderItemID)
                .Except(newOrderItemQuantities.Keys)
                .Concat(splitOrder.OrderItems.Where(oc => oc.Quantity == 0).Select(oc => oc.OrderItemID))
                .Distinct()
                .ToList();

            foreach (long orderItemID in notPresentOrderItemIDs)
            {
                splitOrder.OrderItems.Remove(splitOrder.OrderItems.First(oi => oi.OrderItemID == orderItemID));
            }

            // Remove any order items from the original order that have a Quantity of 0
            IList<long> originalOrderItemsToRemove = originalOrder.OrderItems
                .Where(oc => oc.Quantity == 0)
                .Select(oc => oc.OrderItemID)
                .ToList();

            foreach (long orderItemID in originalOrderItemsToRemove)
            {
                originalOrder.OrderItems.Remove(originalOrder.OrderItems.First(oi => oi.OrderItemID == orderItemID));
            }
        }
    }
}
