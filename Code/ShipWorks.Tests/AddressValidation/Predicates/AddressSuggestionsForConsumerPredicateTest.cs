using Xunit;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.AddressValidation.Predicates
{
    public class AddressSuggestionsForConsumerPredicateTest
    {
        private AddressSuggestionsForConsumerPredicate predicate;
        private FakePredicateExpression pred;

        public AddressSuggestionsForConsumerPredicateTest()
        {
            pred = new FakePredicateExpression();

            predicate = new AddressSuggestionsForConsumerPredicate(123, "Foo");
        }

        [Fact]
        public void Apply_AddsConsumerIdToExpression()
        {
            predicate.Apply(pred);

            Assert.True(pred.ContainsPredicate(ValidatedAddressFields.ConsumerID == (long)123));
        }

        [Fact]
        public void Apply_AddsPrefixToExpression()
        {
            predicate.Apply(pred);

            Assert.True(pred.ContainsPredicate(ValidatedAddressFields.AddressPrefix == "Foo"));
        }
    }
}
