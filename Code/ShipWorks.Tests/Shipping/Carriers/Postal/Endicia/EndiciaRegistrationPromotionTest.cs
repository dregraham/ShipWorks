using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia
{
    [TestClass]
    public class EndiciaRegistrationPromotionTest
    {
        private readonly EndiciaRegistrationPromotion testObject;

        public EndiciaRegistrationPromotionTest()
        {
            testObject = new EndiciaRegistrationPromotion();
        }

        [TestMethod]
        public void AvailableAvailableRegistrationTypes_ReturnsListWithTwoItems_Test()
        {
            IEnumerable<PostalAccountRegistrationType> registrationAccountTypes = testObject.AvailableRegistrationTypes;

            Assert.AreEqual(2, registrationAccountTypes.Count());
        }

        [TestMethod]
        public void AvailableAvailableRegistrationTypes_ContainsExpeditedRegistrationType_Test()
        {
            IEnumerable<PostalAccountRegistrationType> registrationAccountTypes = testObject.AvailableRegistrationTypes;

            Assert.IsTrue(registrationAccountTypes.Any(r => r == PostalAccountRegistrationType.Expedited));
        }

        [TestMethod]
        public void AvailableAvailableRegistrationTypes_ContainsStandardRegistrationType_Test()
        {
            IEnumerable<PostalAccountRegistrationType> registrationAccountTypes = testObject.AvailableRegistrationTypes;

            Assert.IsTrue(registrationAccountTypes.Any(r => r == PostalAccountRegistrationType.Standard));
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks5_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode(PostalAccountRegistrationType.Expedited);

            Assert.AreEqual("shipworks5", promo);
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks4_WhenRegistrationTypeIsStandard_Test()
        {
            string promo = testObject.GetPromoCode(PostalAccountRegistrationType.Standard);

            Assert.AreEqual("shipworks4", promo);
        }
    }
}
