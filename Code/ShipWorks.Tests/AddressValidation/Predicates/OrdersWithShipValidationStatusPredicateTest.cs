using Xunit;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.AddressValidation.Predicates
{
    public class UnprocessedShipmentsWithShipValidationStatusPredicateTest
    {
        private UnprocessedErrorShipmentsPredicate predicate;
        private FakePredicateExpression pred;

        public UnprocessedShipmentsWithShipValidationStatusPredicateTest()
        {
            pred = new FakePredicateExpression();

            predicate = new UnprocessedErrorShipmentsPredicate();
        }

        [Fact]
        public void Apply_AddsShipValidationStatusToExpression()
        {
            predicate.Apply(pred);

            Assert.True(pred.ContainsPredicate(ShipmentFields.ShipAddressValidationStatus == (int)AddressValidationStatusType.Error));
        }

        [Fact]
        public void Apply_AddsProcessedToExpression()
        {
            predicate.Apply(pred);

            Assert.True(pred.ContainsPredicate(ShipmentFields.Processed == false));
        }

        [Fact]
        public void Apply_AddsVoidedToExpression()
        {
            predicate.Apply(pred);

            Assert.True(pred.ContainsPredicate(ShipmentFields.Voided == false));
        }

        [Fact]
        public void MaximumRows_ReturnsLimitOf50()
        {
            int result = predicate.MaximumRows;
            Assert.Equal(50, result);
        }
    }
}
