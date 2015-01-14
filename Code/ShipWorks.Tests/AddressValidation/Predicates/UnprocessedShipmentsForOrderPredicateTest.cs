using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.AddressValidation.Predicates
{
    [TestClass]
    public class UnprocessedShipmentsForOrderPredicateTest
    {
        private UnprocessedShipmentsForOrderPredicate predicate;
        private FakePredicateExpression pred;

        [TestInitialize]
        public void Setup()
        {
            pred = new FakePredicateExpression();

            predicate = new UnprocessedShipmentsForOrderPredicate(123);
        }

        [TestMethod]
        public void Apply_AddsConsumerIdToExpression()
        {
            predicate.Apply(pred);

            Assert.IsTrue(pred.ContainsPredicate(ShipmentFields.OrderID == (long)123));
        }

        [TestMethod]
        public void Apply_AddsPrefixToExpression()
        {
            predicate.Apply(pred);

            Assert.IsTrue(pred.ContainsPredicate(ShipmentFields.Processed == false));
        }
    }
}
