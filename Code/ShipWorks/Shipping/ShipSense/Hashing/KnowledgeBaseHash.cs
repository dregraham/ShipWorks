using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.OrderItems;

namespace ShipWorks.Shipping.ShipSense.Hashing
{
    /// <summary>
    /// An implementation of the IKnowledgeBaseHash interface that calculates a Base64 encoded
    /// SHA256 hash value based on the order items' SKU and quantity of each items per SKU.
    /// </summary>
    public class KnowledgebaseHash : StringHash, IKnowledgebaseHash
    {
        /// <summary>
        /// Uses the data in the order to compute a hash to identify an entry in the
        /// ShipSense knowledge base.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="shipSenseUniquenessXml">XML containing info/fields used for creating the unique hash key</param>
        /// <returns>A string value of the computed hash.</returns>
        public string ComputeHash(OrderEntity order, string shipSenseUniquenessXml)
        {
            List<string> itemAttributeNamesToInclude = ItemAttributeNames(shipSenseUniquenessXml);

            // We want to sort the items by SKU then code, so they are always in the same order
            List<OrderItemEntity> sortedItems = order.OrderItems.ToList();
            sortedItems = sortedItems.OrderBy(oi => oi.SKU).ThenBy(oi => oi.Code).ToList();

            // We're going to group the items by item code and SKU, so we can sum the quantities
            List<IGrouping<string, OrderItemEntity>> groupedItemsBySku = sortedItems.GroupBy(i => AddUniquenessParams(i, itemAttributeNamesToInclude)).ToList();

            // Build up the string data for each item now that we have the items sorted and grouped by SKU
            List<string> skuQuantityPair = new List<string>();
            foreach (IGrouping<string, OrderItemEntity> itemGroup in groupedItemsBySku)
            {
                // If the item key is not something, then we don't want to add it because it would just be the
                // quantity field and that isn't helpful
                if (!string.IsNullOrWhiteSpace(itemGroup.Key))
                {
                    // Since the key already incorporates the item's code and key, each entry 
                    // will be in the format of Code-SKU-TotalQty
                    skuQuantityPair.Add(string.Format("{0}{1}", itemGroup.Key, itemGroup.Sum(g => g.Quantity)));
                }
            }
            
            // Create a single string representing the SKU/Quantity pairs that will be 
            // used to compute the hash
            string valueToHash = string.Join("|", skuQuantityPair);

            // Salt the hash, so it's a little more difficult to crack
            return Hash(valueToHash, "BananaHammock7458");
        }

        /// <summary>
        /// Generates a uniqueness string for a given order item entity.  Includes item level properties like sku, code.
        /// If an item attribute's name matches the list of ship sense attribute names to include, name-description is added to the
        /// string.
        /// </summary>
        /// <param name="orderItemEntity">The OrderItem.</param>
        /// <param name="itemAttributeNamesToInclude">The list of attribute names to include in the string.</param>
        private string AddUniquenessParams(OrderItemEntity orderItemEntity, List<string> itemAttributeNamesToInclude)
        {
            string uniqueness = string.Empty;
            
            // Find the item attributes that match the list of configured attribute names.
            List<OrderItemAttributeEntity> matchingOrderItemAttributes = (from oia in orderItemEntity.OrderItemAttributes
                                                                          join name in itemAttributeNamesToInclude on oia.Name.ToUpperInvariant() equals name.ToUpperInvariant()
                                                                          select oia).ToList();

            // If there are no code, sku, and no matching item attributes, just return
            if (string.IsNullOrWhiteSpace(orderItemEntity.Code) &&
                string.IsNullOrWhiteSpace(orderItemEntity.SKU) &&
                !matchingOrderItemAttributes.Any())
            {
                return string.Empty;
            }

            // Add code and sku
            uniqueness = AddUniquenessParam("", orderItemEntity.Code, "");
            uniqueness += AddUniquenessParam("", orderItemEntity.SKU, "-");

            // Add each attribute name and description
            foreach (OrderItemAttributeEntity orderItemAttributeEntity in matchingOrderItemAttributes.OrderBy(oia => oia.Name))
            {
                uniqueness += AddUniquenessParam(orderItemAttributeEntity.Name.ToUpperInvariant(), orderItemAttributeEntity.Description.ToUpperInvariant(), "-");
            }

            return uniqueness;
        }

        /// <summary>
        /// Returns a formatted string based on the given key, value, and separator.
        /// </summary>
        /// <param name="key">The key of the key/value pair.</param>
        /// <param name="value">The value of the key/value pair.  If this value is null or white space, string.Empty is returned.</param>
        /// <param name="separator">The separator for the key/value pair.  key-value, key|value, etc...</param>
        private static string AddUniquenessParam(string key, string value, string separator)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return string.Format("{0}{1}{2}{3}", key, separator, value, separator);
        }

        /// <summary>
        /// Returns a list of names used to determine uniqueness string.  Only the names in this list are used when
        /// creating the uniqueness string.
        /// </summary>
        /// <param name="shipSenseUniquenessXml">The ShipSense uniqueness settings xml as a string.</param>
        private static List<string> ItemAttributeNames(string shipSenseUniquenessXml)
        {
            List<string> itemAttributeNamesToInclude = new List<string>();

            if (!string.IsNullOrWhiteSpace(shipSenseUniquenessXml))
            {
                try
                {
                    XElement shipSenseUniquenessXElement = XElement.Parse(shipSenseUniquenessXml);
                    itemAttributeNamesToInclude = shipSenseUniquenessXElement
                                                    .Descendants("Name")
                                                    .Select(n => n.Value.ToUpperInvariant())
                                                    .OrderBy(n => n).ToList();
                }
                catch (InvalidOperationException ex)
                {

                    throw new ShipSenseException("ShipSense was unable to determine its settings.", ex);
                }
            }

            return itemAttributeNamesToInclude;
        }

    }
}
