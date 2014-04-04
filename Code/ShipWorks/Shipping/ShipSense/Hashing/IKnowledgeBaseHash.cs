using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipSense.Hashing
{
    public interface IKnowledgebaseHash
    {
        /// <summary>
        /// Uses the data in the order to compute a hash to identify an entry in the 
        /// ShipSense knowledge base.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="shipSenseUniquenessXml">XML containing info/fields used for creating the unique hash key</param>
        /// <returns>A KnowledgebaseHashResult instance containing the value of the computed hash and whether it is valid.</returns>
        KnowledgebaseHashResult ComputeHash(OrderEntity order, string shipSenseUniquenessXml);
    }
}
