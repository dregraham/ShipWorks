using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
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
        [NDependIgnoreLongMethod]
        public IEnumerable<ShipSenseOrderItemKey> GetKeys(IEnumerable<OrderItemEntity> orderItems, List<string> propertyNames, List<string> attributeNames)
        {
            List<ShipSenseOrderItemKey> keys = new List<ShipSenseOrderItemKey>();
            
            // Sort the order items, so the same keys are generated regardless of the sequence the order items are in
            IEnumerable<OrderItemEntity> sortedItems = GetSortedOrderItems(orderItems);

            foreach (OrderItemEntity item in sortedItems)
            {
                ShipSenseOrderItemKey key = new ShipSenseOrderItemKey { Quantity = item.Quantity };

                foreach (string property in propertyNames.OrderBy(p => p))
                {
                    IEntityFieldCore field = item.Fields[property];
                    if (field == null || item.Fields[property] == null)
                    {
                        key.Add(property, string.Empty);
                    }
                    else
                    {
                        if (field.DataType == typeof (decimal))
                        {
                            decimal currentValue = (decimal) item.Fields[property].CurrentValue;
                            key.Add(property, currentValue.ToString("N4"));
                        }
                        else
                        {
                            object currentValue = item.Fields[property].CurrentValue;
                            key.Add(property, currentValue == null ? string.Empty : currentValue.ToString());
                        }
                    }
                }
                
                // Find the item attributes that match the list of attribute names provided.
                List<OrderItemAttributeEntity> matchingOrderItemAttributes = (item.OrderItemAttributes
                                                                                  .Join(attributeNames, oia => oia.Name.ToUpperInvariant(), name => name.ToUpperInvariant(), (oia, name) => oia))
                                                                                  .ToList();

                // Now that we have the matching attributes, we need to sort the attributes by name (so we always have the
                // same order) and add each attribute name and description to our key
                foreach (OrderItemAttributeEntity orderItemAttributeEntity in matchingOrderItemAttributes.OrderBy(attribute => attribute.Name))
                {
                    if (!string.IsNullOrWhiteSpace(orderItemAttributeEntity.Description))
                    {
                        // Ignore any entries that have an empty description
                        key.Add(orderItemAttributeEntity.Name.ToUpperInvariant(), orderItemAttributeEntity.Description.ToUpperInvariant());
                    }
                }

                keys.Add(key);
            }

            // We have the individual order item keys, but now we need to group the items into
            // unique values based on the key in order to get a total sum of quantity
            return GroupIntoUniqueItems(keys);
        }

        /// <summary>
        /// Uses the collection of keys provided to groups them into unique items based on the KeyValue
        /// property. Any duplicates are consolidated into a single entry and the quantities are summed.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>A collection of ShipSenseOrderItemKey where the key values are all distinct. </returns>
        private IEnumerable<ShipSenseOrderItemKey> GroupIntoUniqueItems(IEnumerable<ShipSenseOrderItemKey> keys)
        {
            List<ShipSenseOrderItemKey> uniqueItemKeys = new List<ShipSenseOrderItemKey>();

            // Group the items based on the keys and sum the quantity values
            List<IGrouping<string, ShipSenseOrderItemKey>> groupedKeys = keys.GroupBy(k => k.KeyValue).ToList();
            foreach (IGrouping<string, ShipSenseOrderItemKey> keyGroup in groupedKeys)
            {
                // Copy the first item in the key group to grab the identifier data and update 
                // the quantity to reflect the total quantity for this key
                ShipSenseOrderItemKey key = new ShipSenseOrderItemKey(keyGroup.FirstOrDefault());
                key.Quantity = (keyGroup.Sum(g => g.Quantity));

                // Add the aggregated key to the list to be returned
                uniqueItemKeys.Add(key);
            }

            return uniqueItemKeys;
        }

        /// <summary>
        /// Gets the sorted order items. The actual sequence the criteria is applied is not really important 
        /// here; we just need the items being ordered in a consistent manner.
        /// </summary>
        private static IEnumerable<OrderItemEntity> GetSortedOrderItems(IEnumerable<OrderItemEntity> orderItems)
        {
            List<OrderItemEntity> sortedItems = new List<OrderItemEntity>(orderItems).OrderBy(i => i.SKU)
                                                                                               .ThenBy(i => i.Code).ThenBy(i => i.Name).ThenBy(i => i.ISBN).ThenBy(i => i.Description)
                                                                                               .ThenBy(i => i.UPC).ThenBy(i => i.Location).ThenBy(i => i.UnitPrice).ThenBy(i => i.UnitCost)
                                                                                               .ThenBy(i => i.LocalStatus).ToList();

            return sortedItems;
        }
    }
}
