using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.AddressValidation.Predicates
{
    [TestClass]
    public class OrdersWithShipValidationStatusPredicateTest
    {
        private OrdersWithShipValidationStatusPredicate predicate;
        private FakePredicateExpression pred;

        [TestInitialize]
        public void Setup()
        {
            pred = new FakePredicateExpression();

            predicate = new OrdersWithShipValidationStatusPredicate(AddressValidationStatusType.Error);
        }

        [TestMethod]
        public void Apply_AddsShipValidationStatusToExpression()
        {
            predicate.Apply(pred);

            Assert.IsTrue(pred.ContainsPredicate(OrderFields.ShipAddressValidationStatus == (int) AddressValidationStatusType.Error));
        }

        [TestMethod]
        public void MaximumRows_ReturnsLimitOf50()
        {
            int result = predicate.MaximumRows;
            Assert.AreEqual(50, result);
        }
    }

    [TestClass]
    public class UnprocessedShipmentsWithShipValidationStatusPredicateTest
    {
        private UnprocessedShipmentsWithShipValidationStatusPredicate predicate;
        private FakePredicateExpression pred;

        [TestInitialize]
        public void Setup()
        {
            pred = new FakePredicateExpression();

            predicate = new UnprocessedShipmentsWithShipValidationStatusPredicate(AddressValidationStatusType.Error);
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
