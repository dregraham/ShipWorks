using Xunit;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.AddressValidation.Predicates
{
    public class UnprocessedShipmentsWithShipValidationStatusPredicateTest
    {
        private UnprocessedErrorShipmentsPredicate predicate;
        private FakePredicateExpression pred;

        [TestInitialize]
        public void Setup()
        {
            pred = new FakePredicateExpression();

            predicate = new UnprocessedErrorShipmentsPredicate();
        }

        [Fact]
        public void Apply_AddsShipValidationStatusToExpression()
        {
            predicate.Apply(pred);

            Assert.IsTrue(pred.ContainsPredicate(ShipmentFields.ShipAddressValidationStatus == (int)AddressValidationStatusType.Error));
        }

        [Fact]
        public void Apply_AddsProcessedToExpression()
        {
            predicate.Apply(pred);

            Assert.IsTrue(pred.ContainsPredicate(ShipmentFields.Processed == false));
        }

        [Fact]
        public void MaximumRows_ReturnsLimitOf50()
        {
            int result = predicate.MaximumRows;
            Assert.AreEqual(50, result);
        }
    }
}
