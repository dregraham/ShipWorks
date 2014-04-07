using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipSense.Hashing
{
    /// <summary>
    /// A factory for creating ShipSenseOrderItemKey objects.
    /// </summary>
    public class ShipSenseOrderItemKeyFactory : IShipSenseOrderItemKeyFactory
    {
        /// <summary>
        /// Creates a list of ShipSenseOrderItemKey instances for the given order items. The ShipSenseOrderItemKey instances
        /// will be configured with the names/values of the properties and attributes provided.
        /// </summary>
        public IEnumerable<ShipSenseOrderItemKey> GetKeys(IEnumerable<OrderItemEntity> orderItems, List<string> propertyNames, List<string> attributeNames)
        {
            List<ShipSenseOrderItemKey> keys = new List<ShipSenseOrderItemKey>();
            
            // Sort the order items, so the same keys are generated regardless of the sequence the order items are in
            IEnumerable<OrderItemEntity> sortedItems = GetSortedOrderItems(orderItems);
            foreach (OrderItemEntity item in sortedItems)
            {
                ShipSenseOrderItemKey key = new ShipSenseOrderItemKey(item.Quantity);

                foreach (string property in propertyNames.OrderBy(p => p.ToUpperInvariant()))
                {
                    key.Add(property, item.Fields[property].CurrentValue.ToString());
                }
                
                // Find the item attributes that match the list of attribute names provided.
                List<OrderItemAttributeEntity> matchingOrderItemAttributes = (item.OrderItemAttributes
                                                                                  .Join(attributeNames, oia => oia.Name.ToUpperInvariant(), name => name.ToUpperInvariant(), (oia, name) => oia))
                                                                                  .ToList();

                // Now that we have the matching attributes, we need to sort the attributes by name (so we always have the
                // same order) and add each attribute name and description to our key
                foreach (OrderItemAttributeEntity orderItemAttributeEntity in matchingOrderItemAttributes.OrderBy(attribute => attribute.Name))
                {
                    key.Add(orderItemAttributeEntity.Name.ToUpperInvariant(), orderItemAttributeEntity.Description.ToUpperInvariant());
                }

                keys.Add(key);
            }

            return keys;
        }

        /// <summary>
        /// Gets the sorted order items. The actual sequence the criteria is applied is not really important 
        /// here; we just need the items being ordered in a consistent manner.
        /// </summary>
        private static IEnumerable<OrderItemEntity> GetSortedOrderItems(IEnumerable<OrderItemEntity> orderItems)
        {
            return new List<OrderItemEntity>(orderItems).OrderBy(i => i.SKU)
                                                        .ThenBy(i => i.Code).ThenBy(i => i.Name).ThenBy(i => i.ISBN).ThenBy(i => i.Description)
                                                        .ThenBy(i => i.UPC).ThenBy(i => i.Location).ThenBy(i => i.UnitPrice).ThenBy(i => i.UnitCost)
                                                        .ThenBy(i => i.LocalStatus);
        }
    }
}
