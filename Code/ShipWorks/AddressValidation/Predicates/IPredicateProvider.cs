using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Pre-built predicate expressions
    /// </summary>
    public interface IPredicateProvider
    {
        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        void Apply(IPredicateExpression predicate);
    }
}