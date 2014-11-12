using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.Registration.Promotion
{
    [TestClass]
    public class Express1OnlyRegistrationPromotionTest
    {
        private readonly Express1OnlyRegistrationPromotion testObject;

        public Express1OnlyRegistrationPromotionTest()
        {
            testObject = new Express1OnlyRegistrationPromotion();
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
        public void GetPromoCode_ReturnsShipWorks7_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode(PostalAccountRegistrationType.Expedited);

            Assert.AreEqual("ShipWorks7", promo);
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks7_WhenRegistrationTypeIsStandard_Test()
        {
            string promo = testObject.GetPromoCode(PostalAccountRegistrationType.Standard);

            Assert.AreEqual("ShipWorks7", promo);
        }
    }
}
