using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.AddressValidation.Predicates
{
    [TestClass]
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

        [TestMethod]
        public void Apply_AddsShipValidationStatusToExpression()
        {
            predicate.Apply(pred);

            Assert.IsTrue(pred.ContainsPredicate(ShipmentFields.ShipAddressValidationStatus == (int)AddressValidationStatusType.Error));
        }

        [TestMethod]
        public void Apply_AddsProcessedToExpression()
        {
            predicate.Apply(pred);

            Assert.IsTrue(pred.ContainsPredicate(ShipmentFields.Processed == false));
        }

        [TestMethod]
        public void MaximumRows_ReturnsLimitOf50()
        {
            int result = predicate.MaximumRows;
            Assert.AreEqual(50, result);
        }
    }
}
