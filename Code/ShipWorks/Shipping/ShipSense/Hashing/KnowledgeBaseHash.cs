using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

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
            List<string> itemAttributeNamesToInclude = XElement.Parse(shipSenseUniquenessXml).Descendants("Name").Select(n => n.Value).ToList();

            // We want to sort the items by SKU, so they are always in the same order
            order.OrderItems.Sort(OrderItemFields.SKU.FieldIndex, ListSortDirection.Ascending);
            List<OrderItemEntity> sortedItems = order.OrderItems.ToList();

            // We're going to group the items by item code and SKU, so we can sum the quantities
            List<IGrouping<string, OrderItemEntity>> groupedItemsBySku = sortedItems.GroupBy(i => string.Format("{0}-{1}", i.Code, i.SKU)).ToList();

            // Build up the string data for each item now that we have the items sorted and grouped by SKU
            List<string> skuQuantityPair = new List<string>();
            foreach (IGrouping<string, OrderItemEntity> itemGroup in groupedItemsBySku)
            {
                // Since the key already incorporates the item's code and key, each entry 
                // will be in the format of Code-SKU-TotalQty
                skuQuantityPair.Add(string.Format("{0}-{1}",itemGroup.Key, itemGroup.Sum(g => g.Quantity)));
            }
            
            // Create a single string representing the SKU/Quantity pairs that will be 
            // used to compute the hash
            string valueToHash = string.Join("|", skuQuantityPair);

            // Salt the hash, so it's a little more difficult to crack
            return Hash(valueToHash, "BananaHammock7458");
        }
    }
}
