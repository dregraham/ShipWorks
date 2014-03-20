using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.ShipSense.Hashing
{
    /// <summary>
    /// An implementation of the IKnowledgeBaseHash interface that calculates a Base64 encoded 
    /// SHA256 hash value based on the order items' SKU and quantity of each items per SKU.
    /// </summary>
    public class KnowledgeBaseHash : IKnowledgeBaseHash
    {
        /// <summary>
        /// Uses the data in the order to compute a hash to identify an entry in the
        /// ShipSense knowledge base.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>A string value of the computed hash.</returns>
        public string ComputeHash(OrderEntity order)
        {
            // We want to sort the items by SKU, so they are always in the same order
            order.OrderItems.Sort(OrderItemFields.SKU.FieldIndex, ListSortDirection.Ascending);
            List<OrderItemEntity> sortedItems = order.OrderItems.ToList();

            // We're going to group the items by SKU, so we can sum the quantities
            List<IGrouping<string, OrderItemEntity>> groupedItemsBySku = sortedItems.GroupBy(i => i.SKU).ToList();

            // Build up the string data for each item now that we have the items sorted and grouped by SKU
            List<string> skuQuantityPair = new List<string>();
            foreach (IGrouping<string, OrderItemEntity> itemGroup in groupedItemsBySku)
            {
                // Each entry will be in the format of SKU-TotalQty
                skuQuantityPair.Add(string.Format("{0}-{1}", itemGroup.Key, itemGroup.Sum(g => g.Quantity)));
            }
            
            // Create a single string representing the SKU/Quantity pairs that will be 
            // used to compute the hash
            string valueToHash = string.Join("|", skuQuantityPair);
            return Hash(valueToHash);
        }

        /// <summary>
        /// Hashes the raw value into Base64 string.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        /// <returns>A Base64 string of the hashed raw value.</returns>
        private string Hash(string rawValue)
        {
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                // Note: we could use an attribute of the store (e.g. license, store name, etc.) that is 
                // globally unique (outside of just ShipWOrks) as the salt to guarantee uniqueness across 
                // stores in the event of SKU/quantity collisions.
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawValue));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
