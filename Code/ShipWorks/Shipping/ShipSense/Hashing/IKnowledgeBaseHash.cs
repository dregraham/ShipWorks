using System.Collections.Generic;

namespace ShipWorks.Shipping.ShipSense.Hashing
{
    public interface IKnowledgebaseHash
    {
        /// <summary>
        /// Uses the data in the order to compute a hash to identify an entry in the
        /// ShipSense knowledge base.
        /// </summary>
        /// <param name="keys">ShipSenseOrderItemKey values containing info the info used for creating the unique hash key</param>
        /// <returns>A KnowledgebaseHashResult instance containing the value of the computed hash and whether it is valid.</returns>
        KnowledgebaseHashResult ComputeHash(IEnumerable<ShipSenseOrderItemKey> keys);
    }
}
