using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;

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
         /// <param name="keys">ShipSenseOrderItemKey values containing info the info used for creating the unique hash key</param>
         /// <returns>A KnowledgebaseHashResult instance containing the value of the computed hash and whether it is valid.</returns>
         public KnowledgebaseHashResult ComputeHash(IEnumerable<ShipSenseOrderItemKey> keys)
         {
             IEnumerable<ShipSenseOrderItemKey> shipSenseKeys = keys as IList<ShipSenseOrderItemKey> ?? keys.ToList();

             if (!shipSenseKeys.Any() || !shipSenseKeys.All(k => k.IsValid()))
             {
                 return new KnowledgebaseHashResult(false, string.Empty);
             }

             return CreateValidHashResult(shipSenseKeys);
         }

         /// <summary>
         /// Uses the unique items provided to create a valid hash result.
         /// </summary>
         /// <param name="keys">The keys.</param>
         /// <returns>A KnowledgebaseHashResult instance containing the value of the computed hash and whether it is valid.</returns>
         private KnowledgebaseHashResult CreateValidHashResult(IEnumerable<ShipSenseOrderItemKey> keys)
         {
             // Create a single string representing all of the keys and the quantity of each
             string valueToHash = string.Join("|", keys.Select(k => string.Format("{0}-{1}", k.KeyValue, k.Quantity)));

             // Salt the hash, so it's a little more difficult to crack
             string hash = Hash(valueToHash, "BananaHammock7458");

             return new KnowledgebaseHashResult(true, hash);
         }
    }
}
