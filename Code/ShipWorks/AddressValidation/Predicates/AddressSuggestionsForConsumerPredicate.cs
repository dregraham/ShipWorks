using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Get address suggestions for the specific consumer
    /// </summary>
    public class AddressSuggestionsForConsumerPredicate : IPredicateProvider
    {
        private readonly long consumerId;
        private readonly string prefix;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddressSuggestionsForConsumerPredicate(long consumerId, string prefix)
        {
            this.consumerId = consumerId;
            this.prefix = prefix;
        }

        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            predicate.Add(ValidatedAddressFields.ConsumerID == consumerId)
                .AddWithAnd(ValidatedAddressFields.AddressPrefix == prefix);
        }
    }
}