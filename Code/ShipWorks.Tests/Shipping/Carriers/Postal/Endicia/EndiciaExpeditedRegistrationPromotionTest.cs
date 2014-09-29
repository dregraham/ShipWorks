using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia
{
    [TestClass]
    public class EndiciaExpeditedRegistrationPromotionTest
    {
        private readonly EndiciaExpeditedRegistrationPromotion testObject;

        public EndiciaExpeditedRegistrationPromotionTest()
        {
            testObject = new EndiciaExpeditedRegistrationPromotion();
        }

        [TestMethod]
        public void AvailableAvailableAccountTypes_ReturnsListWithBothItems_Test()
        {
            IEnumerable<PostalAccountRegistrationType> registrationAccountTypes = testObject.AvailableAccountTypes;

            Assert.AreEqual(2, registrationAccountTypes.Count());
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode(PostalAccountRegistrationType.Expedited);

            Assert.AreEqual("shipworks5", promo);
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks3_WhenRegistrationTypeIsStandard_Test()
        {
            string promo = testObject.GetPromoCode(PostalAccountRegistrationType.Standard);

            Assert.AreEqual("shipworks4", promo);
        }
    }
}
