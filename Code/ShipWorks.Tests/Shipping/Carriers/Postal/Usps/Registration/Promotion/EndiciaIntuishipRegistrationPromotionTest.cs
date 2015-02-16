using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    [TestClass]
    public class EndiciaIntuishipRegistrationPromotionTest
    {
        private readonly EndiciaIntuishipRegistrationPromotion testObject;

        public EndiciaIntuishipRegistrationPromotionTest()
        {
            testObject = new EndiciaIntuishipRegistrationPromotion();
        }

        [TestMethod]
        public void GetPromoCode_ReturnsShipWorks5_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.AreEqual("ShipWorks5", promo);
        }
    }
}
