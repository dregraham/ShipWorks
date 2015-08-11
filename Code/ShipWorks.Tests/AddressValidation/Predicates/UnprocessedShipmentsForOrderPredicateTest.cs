using Xunit;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.AddressValidation.Predicates
{
    public class UnprocessedShipmentsForOrderPredicateTest
    {
        private UnprocessedShipmentsForOrderPredicate predicate;
        private FakePredicateExpression pred;

        public UnprocessedShipmentsForOrderPredicateTest()
        {
            pred = new FakePredicateExpression();

            predicate = new UnprocessedShipmentsForOrderPredicate(123);
        }

        [Fact]
        public void Apply_AddsConsumerIdToExpression()
        {
            predicate.Apply(pred);

            Assert.True(pred.ContainsPredicate(ShipmentFields.OrderID == (long)123));
        }

        [Fact]
        public void Apply_AddsPrefixToExpression()
        {
            predicate.Apply(pred);

            Assert.True(pred.ContainsPredicate(ShipmentFields.Processed == false));
        }
    }
}
