using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration.Promotion
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
        public void GetPromoCode_ReturnsShipWorks7_WhenRegistrationTypeIsExpedited_Test()
        {
            string promo = testObject.GetPromoCode();

            Assert.AreEqual("ShipWorks7", promo);
        }
    }
}
