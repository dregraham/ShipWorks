using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.Registration
{
    [TestClass]
    public class StampsExpeditedRegistrationPromotionTest
    {
        private readonly StampsExpeditedRegistrationPromotion testObject;

        public StampsExpeditedRegistrationPromotionTest()
        {
            testObject = new StampsExpeditedRegistrationPromotion();
        }

        [TestMethod]
        public void AvailableAvailableRegistrationTypes_ReturnsListWithSingleItem_Test()
        {
            IEnumerable<PostalAccountRegistrationType> registrationAccountTypes = testObject.AvailableRegistrationTypes;

            Assert.AreEqual(1, registrationAccountTypes.Count());
        }

        [TestMethod]
        public void AvailableAvailableRegistrationTypes_ContainsExpeditedRegistrationType_Test()
        {
            IEnumerable<PostalAccountRegistrationType> registrationAccountTypes = testObject.AvailableRegistrationTypes;

            Assert.AreEqual(PostalAccountRegistrationType.Expedited, registrationAccountTypes.First());
        }
    }
}
