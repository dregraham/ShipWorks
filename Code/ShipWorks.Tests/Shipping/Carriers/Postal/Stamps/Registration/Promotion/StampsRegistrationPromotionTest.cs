using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.Registration.Promotion
{
    [TestClass]
    public class StampsRegistrationPromotionTest
    {
        private readonly StampsIntuishipRegistrationPromotion testObject;

        public StampsRegistrationPromotionTest()
        {
            testObject = new StampsIntuishipRegistrationPromotion();
        }

        [TestMethod]
        public void AvailableAvailableRegistrationTypes_ReturnsListWithTwoItems_Test()
        {
            IEnumerable<PostalAccountRegistrationType> registrationAccountTypes = testObject.AvailableRegistrationTypes;

            Assert.AreEqual(1, registrationAccountTypes.Count());
        }

        [TestMethod]
        public void AvailableAvailableRegistrationTypes_ContainsExpeditedRegistrationType_Test()
        {
            IEnumerable<PostalAccountRegistrationType> registrationAccountTypes = testObject.AvailableRegistrationTypes;

            Assert.IsTrue(registrationAccountTypes.Any(r => r == PostalAccountRegistrationType.Expedited));
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode(PostalAccountRegistrationType.Expedited);

            Assert.AreEqual("ShipWorks3", promo);
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsStandard_Test()
        {
            string promo = testObject.GetPromoCode(PostalAccountRegistrationType.Standard);

            Assert.AreEqual("ShipWorks3", promo);
        }
    }
}
